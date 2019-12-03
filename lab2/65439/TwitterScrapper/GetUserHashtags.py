import time
import sys
import csv

from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from bs4 import BeautifulSoup


path_arg = sys.argv[1]
user_name_arg = sys.argv[2]


def user_hashtags(path, user_name, since, to):
    twitter_username = user_name
    browser_driver = webdriver.Chrome(executable_path=path)
    base_url = "https://twitter.com/search?f=tweets&vertical=default&q="
    who = "from%3A" + twitter_username
    since_when = " since%3A" + since
    to_when = " until%3A" + to

    url = base_url + who + since_when + to_when

    browser_driver.get(url)
    time.sleep(1)

    hashtags = list()

    twitter_elm = browser_driver.find_elements_by_class_name("tweet")

    last_height = browser_driver.execute_script("return document.body.scrollHeight")

    while True:
        browser_driver.execute_script("window.scrollTo(0, document.body.scrollHeight)")
        time.sleep(2)
        new_height = browser_driver.execute_script("return document.body.scrollHeight")
        twitter_elm = browser_driver.find_elements_by_class_name("tweet")
        if new_height == last_height:
            break
        last_height = new_height
        for post in twitter_elm:
            username = post.find_element_by_class_name("username")
            if username.text.lower() == "@" + twitter_username.lower():
                tweet = post.find_element_by_class_name("tweet-text")
                hashflag = tweet.find_elements_by_class_name("twitter-hashtag")
                if hashflag:
                    if type(hashflag) == list:
                        for h in hashflag:
                            hashtags.append(h.text)
                    else:
                        hashtags.append(hashflag.text)

    hashtags = list(dict.fromkeys(hashtags))
    print(hashtags)

    with open("recent_hashtags.csv", "w", newline='') as myfile:
        wr = csv.writer(myfile, quoting=csv.QUOTE_ALL, delimiter="\n")
        wr.writerow(hashtags)

    browser_driver.quit()
    sys.exit(1)


user_hashtags(path_arg, user_name_arg, sys.argv[3], sys.argv[4])

#python GetUserHashtags.py "C:\Users\ppmic\Desktop\chromedriver.exe" "Marvel" "2019-10-27" "2019-11-19"
