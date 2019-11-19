import json
import bs4
import requests
import sys
import base64


class Base64Encoder(json.JSONEncoder):
    def default(self, o):
        if isinstance(o, bytes):
            return base64.b64encode(o).decode()
        return json.JSONEncoder.default(self, o)


def get_as_base64(url):
    return base64.b64encode(requests.get(url).content)


brand = sys.argv[1]
model = sys.argv[2]
startYear = sys.argv[3]
location = sys.argv[4]
priceFrom = sys.argv[5]
priceTo = sys.argv[6]
endYear = sys.argv[7]
mileageFrom = sys.argv[8]
mileageTo = sys.argv[9]

path = 'https://www.otomoto.pl/osobowe/' + brand + '/' + model + '/od-' + startYear + '/' + location + '/?search%5Bfilter_float_price%3Afrom%5D=' + priceFrom + '&search%5Bfilter_float_price%3Ato%5D=' + priceTo + '&search%5Bfilter_float_year%3Ato%5D=' + endYear + '&search%5Bfilter_float_mileage%3Afrom%5D=' + mileageFrom + '&search%5Bfilter_float_mileage%3Ato%5D=' + mileageTo
res = requests.get(path)
res.raise_for_status()

print(path)

carSoup = bs4.BeautifulSoup(res.text, features='lxml')

deb = carSoup.select('.page')
priceList = []
titleList = []
yearList = []
mileageList = []
engineCapacityList = []
fuelTypeList = []
imageList = []

if int(deb.__len__()) == 0:
    res = requests.get(path)
    res.raise_for_status()
    currentPage = bs4.BeautifulSoup(res.text, features='lxml')
    carList = currentPage.select('article.offer-item')

    for car in carList:
        price = car.find('span', class_='offer-price__number').text.strip().replace(' ', '').replace('\n', ' ')
        priceList.append(price)
        title = car.find('a', class_='offer-title__link').text.strip()
        titleList.append(title)
        image = car.find('img').get('data-src')
        imageList.append(image)

        paramList = ['year', 'mileage', 'engine_capacity', 'fuel_type']
        for param in paramList:
            currentParameter = car.find('li', {'data-code': param})
            if currentParameter:
                if param == 'year':
                    yearList.append(currentParameter.text.strip())
                elif param == 'mileage':
                    mileageList.append(currentParameter.text.strip())
                elif param == 'engine_capacity':
                    engineCapacityList.append(currentParameter.text.strip())
                else:
                    fuelTypeList.append(currentParameter.text.strip())
            else:
                continue

else:
    lastPage = int(carSoup.select('.page')[-1].text)

    for i in range(0, lastPage):
        res = requests.get(path + '?page=' + str(i))
        res.raise_for_status()
        currentPage = bs4.BeautifulSoup(res.text, features='lxml')
        carList = currentPage.select('article.offer-item')

        for car in carList:
            price = car.find('span', class_='offer-price__number').text.strip().replace(' ', '').replace('\n', ' ')
            priceList.append(price)
            title = car.find('a', class_='offer-title__link').text.strip()
            titleList.append(title)
            image = car.find('img').get('data-src')
            imageList.append(image)

            paramList = ['year', 'mileage', 'engine_capacity', 'fuel_type']
            for param in paramList:
                currentParameter = car.find('li', {'data-code': param})
                if currentParameter:
                    if param == 'year':
                        yearList.append(currentParameter.text.strip())
                    elif param == 'mileage':
                        mileageList.append(currentParameter.text.strip())
                    elif param == 'engine_capacity':
                        engineCapacityList.append(currentParameter.text.strip())
                    else:
                        fuelTypeList.append(currentParameter.text.strip())
                else:
                    continue

fileDisplay = [{'Price': p, 'Title': t, 'Year': y, 'Mileage': m, 'Engine_Capacity': e, 'Fuel_Type': f, 'Image': get_as_base64(i)} for
               p, t, y, m, e, f, i in zip(priceList, titleList, yearList, mileageList, engineCapacityList, fuelTypeList, imageList)]

with open('car.json', 'w', encoding='utf-8') as file:
    json.dump(fileDisplay, file, ensure_ascii=False, indent=4, cls=Base64Encoder)

print('Wynik działania programu znajduje się w pliku car.json')
