const request = require('request');
const cheerio = require('cheerio');
const image2base64 = require('image-to-base64');
const fs = require('fs');


const args = process.argv.slice(2);
const brand = args[0];
const model = args[1];
const priceFrom = args[2];
const priceTo = args[3];
const fuel = args[4];
const location = args[5];

var announcement;
var $;
var offer = new Object();
var offers = []



scrape(1);
 function scrape(actualPage){

request('https://www.otomoto.pl/osobowe/'+brand+'/'+model+'/'+location+'/'+
'?search[filter_float_price%3Afrom]='+priceFrom+'&search[filter_float_price%3Ato]='+priceTo+
'&search[filter_enum_fuel_type][0]='+fuel+'&page='+actualPage ,
    (error, response, html) => {
        if(!error && response.statusCode == 200 ) {
            $ = cheerio.load(html);            
            announcement = $('.adListingItem').toArray();
            var pg = parseInt($('.page').text(),10);
            
            var numberOfAnnoucements = $('.selected .counter').text()
            numberOfAnnoucements = numberOfAnnoucements.replace("(","").replace(")","")
            console.log("URL:")
            console.log('https://www.otomoto.pl/osobowe/'+brand+'/'+model+'/'+location+'/'+
            '?search[filter_float_price%3Afrom]='+priceFrom+'&search[filter_float_price%3Ato]='+priceTo+
            '&search[filter_enum_fuel_type][0]='+fuel+'&page='+actualPage)



         announcement.forEach( el=> {


            offer.imgURL=$('img', el)
                 .attr('data-src');

               offer.base64=image2base64(offer.imgURL)
                .then((response)=> {
                offer.base64= response
                    offer.car = $('.offer-title__link', el)
                    .text()
                    .replace(/(\r\n|\n|\r|  +)/gm,"");
                
                offer.subtitle = $('.offer-item__subtitle', el)
                    .text()
                    .replace(/(\r\n|\n|\r|  +)/gm,"");
                
                offer.title = $('.offer-item__title', el)
                    .text()
                    .replace(/(\r\n|\n|\r|  +)/gm,"");
    
                offer.year = $('.ds-params-block [data-code=year]', el)
                    .text()
                    .replace(/(\r\n|\n|\r|  +)/gm,"");
    
                offer.mileage = $('.ds-params-block [data-code=mileage]', el)
                    .text()
                    .replace(/(\r\n|\n|\r|  +)/gm,"");
    
                offer.engine_capacity = $('.ds-params-block [data-code=engine_capacity]', el)
                    .text()
                    .replace(/(\r\n|\n|\r|  +)/gm,"");
    
                offer.fuel_type = $('.ds-params-block [data-code=fuel_type]', el)
                    .text()
                    .replace(/(\r\n|\n|\r|  +)/gm,"");
    
                offer.price=$('.offer-price', el)
                    .text()
                    .replace(/(\r\n|\n|\r|  +)/gm,"");

                    offers.push(new Object({
                        car: offer.car,
                        title: offer.subtitle,
                        year: offer.year,
                        mileage: offer.mileage,
                        engine_capacity: offer.engine_capacity,
                        fuel_type: offer.fuel_type,
                        price: offer.price,
                        imgURL: offer.imgURL,
                        base64: offer.base64
                    }));
                    console.log("Rozmiar tablicy: "+offers.length)
                    
                   if(offers.length+1==parseInt(numberOfAnnoucements))
                        fs.writeFileSync('otomoto.json', JSON.stringify(offers));  
                }
 
                    )
                .catch(
                    (error) => {
                        console.log(error); 
                    }
                )                
            });
            if(actualPage<pg)
            {
                actualPage++;
                scrape(actualPage);
            }
            

            

        }});
    }


    

