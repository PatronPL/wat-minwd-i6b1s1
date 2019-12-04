import requests
from PIL import Image
import re
from bs4 import BeautifulSoup
import sys
import base64
import json

url = "https://pl.wiktionary.org/wiki/"
word = sys.argv[1]
all = url+word
html = requests.get(all)
soup = BeautifulSoup(html.text, 'lxml')
meaning = []
line = ''
temp = ''

for h in soup.findAll("div", class_="mw-parser-output"):
        for j in h.findAll(name="dl"):
            for k in j.findAll(name="dd"):
                for l in k.findAll(name="a", href=True):
                    temp = l.text
                    line += ' ' + temp
                if line != '':
                    meaning.append(line)
                line = ''
            if meaning[-1] != "0": meaning.append("0")

finalMeaning = []
counter1 = 0
counter2 = 0

for i in meaning:
    if i == '0': break
    else:
     counter1+=1


for i in meaning:
    if counter1 <= counter2 and i != "0":
        finalMeaning.append(i)
    counter2+=1

listImage = []
images = soup.find_all('img', {'src':re.compile('.jpg')})

for image in images:
    listImage.append(image['src'])

j = 0
listNameImages = []

for i in listImage:
    i = "http:"+i
    print(i)
    img = Image.open(requests.get(i, stream=True).raw)
    img.save('image'+str(j)+'.jpg')
    listNameImages.append('image'+str(j)+'.jpg')
    j+=1

listImageBase64 = []

for i in listNameImages:
    with open(i, "rb") as imageFile:
        base64_bytes = base64.b64encode(imageFile.read())
        base64_string = base64_bytes.decode('utf-8')
        json_data = json.dumps(base64_string, indent=2)
        listImageBase64.append(base64_string)

result = { 'znaczenie':finalMeaning, 'obrazy':listImageBase64 }

with open(str(word)+'.json', 'w', encoding="utf-8") as json_file:
     json_file.write(str(result))

