import time
import sys
import csv

from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from bs4 import BeautifulSoup


#"C:\Users\ppmic\Desktop\chromedriver.exe"

path_arg = sys.argv[1]
user_name_arg = sys.argv[2]


def user_info(path, user_name):
    twitter_username = user_name
    browser_driver = webdriver.Chrome(executable_path=path)
    base_url = "https://twitter.com/"
    query_url = base_url + twitter_username

    browser_driver.get(query_url)
    time.sleep(1)

    user_card = browser_driver.find_element_by_class_name("ProfileHeaderCard")

    soup = BeautifulSoup(user_card.text, "lxml")

    print("User info card:")
    print(soup.text)

    f = open('userinfo.csv', 'w')
    f.write(soup.text)
    f.close()

    browser_driver.quit()
    sys.exit(1)


user_info(path_arg, user_name_arg)


#python GetUserInfo.py "C:\Users\ppmic\Desktop\chromedriver.exe" "Snowden"
