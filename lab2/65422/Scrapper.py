# command to run:
# py Scrapper.py mieszkanie sprzedaz 1000 300000 25 90 4 1990 2000
import json
import sys
import bs4
import requests
import base64

# input params
typeOfBuilding = sys.argv[1]
typeOfContract = sys.argv[2]
minPrice = sys.argv[3]
maxPrice = sys.argv[4]
minSize = sys.argv[5]
maxSize = sys.argv[6]
roomNumber = sys.argv[7]
minYear = sys.argv[8]
maxYear = sys.argv[9]

paramList = ['offer-item-rooms hidden-xs',
             'offer-item-price',
             'hidden-xs offer-item-area',
             'hidden-xs offer-item-price-per-m']

priceList = []
pricePerMeterList = []
areaList = []
titleList = []
locationList = []
imageList = []
numberOfRoomsList = []

path = 'https://www.otodom.pl/' + typeOfContract + '/' + typeOfBuilding + '/' + \
       '?search%5Bfilter_float_price%3Afrom%5D=' + minPrice + \
       '&search%5Bfilter_float_price%3Ato%5D=' + maxPrice + \
       '&search%5Bfilter_float_m%3Afrom%5D=' + minSize + \
       '&search%5Bfilter_float_m%3Ato%5D=' + maxSize + \
       '&search%5Bfilter_enum_rooms_num%5D%5B0%5D=' + roomNumber + \
       '&search%5Bfilter_float_build_year%3Afrom%5D=' + \
       minYear + '&search%5Bfilter_float_build_year%3Ato%5D=' + maxYear
print(path)


class Base64Encoder(json.JSONEncoder):
    def default(self, o):
        if isinstance(o, bytes):
            return base64.b64encode(o).decode()
        return json.JSONEncoder.default(self, o)


def get_as_base64(url):
    return base64.b64encode(requests.get(url).content)


def separate_data(flat_list, param_list, type_building, type_contract):
    for flat in flat_list:

        title = flat.find('span', class_='offer-item-title').text.strip()
        titleList.append(title)

        location = flat.find('p', class_='text-nowrap').text.strip()
        locationList.append(location[location.find(':') + 2:])

        image = flat.find('span', class_='img-cover').get('data-src')
        imageList.append(image)

        for param in param_list:
            current_parameter = flat.find('li', {'class': param})

            if current_parameter:
                if not type_building:
                    pricePerMeterList.append('no data')
                if param == 'offer-item-rooms hidden-xs':
                    numberOfRoomsList.append(current_parameter.text.strip())
                elif param == 'offer-item-price':
                    priceList.append(current_parameter.text.strip())
                elif param == 'hidden-xs offer-item-area':
                    areaList.append(current_parameter.text.strip())
                elif type_building and type_contract and param == 'hidden-xs offer-item-price-per-m':
                    pricePerMeterList.append(current_parameter.text.strip())

            else:
                continue


res = requests.get(path)
res.raise_for_status()
flatSoup = bs4.BeautifulSoup(res.text, features='lxml')
pagers = flatSoup.find(class_='pager')
page_count = 1

if pagers:
    page_count = int(pagers.select('a')[-2].text)

for i in range(1, page_count + 1):
    res = requests.get(path + '?page=' + str(i))
    res.raise_for_status()
    currentPage = bs4.BeautifulSoup(res.text, features='lxml')
    flatList = currentPage.select('article.offer-item')

    separate_data(flat_list=flatList, param_list=paramList,
                  type_building=bool(typeOfBuilding == 'mieszkanie'),
                  type_contract=bool(typeOfContract == 'sprzedaz'))

fileDisplay = [{'Title': title, 'Location': location, 'Price': price, 'Price/m2': pricePerMeter,
                'Rooms': numberOfRooms, 'Area': area, 'Image': get_as_base64(image)}
               for title, location, price, pricePerMeter, numberOfRooms, area, image in
               zip(titleList, locationList, priceList, pricePerMeterList, numberOfRoomsList, areaList, imageList)]

with open('flats.json', 'w', encoding='utf-8') as file:
    json.dump(fileDisplay, file, ensure_ascii=False, indent=4, cls=Base64Encoder)
