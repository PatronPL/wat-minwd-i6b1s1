import time
import sys
import csv

from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from bs4 import BeautifulSoup

path_arg = sys.argv[1]
user_name_arg = sys.argv[2]
login_arg = sys.argv[3]
password_arg = sys.argv[4]
followers_arg = int(sys.argv[5])

def user_followers(path, user_name, login, password, followers):
    path = path
    twitter_username = user_name
    the_login = login
    the_password = password
    how_many_followers = followers

    browser_driver = webdriver.Chrome(executable_path=path)
    base_url = "https://twitter.com/"
    query_url = base_url + twitter_username + "/followers"

    followers_site = browser_driver.get(query_url)
    username_field = browser_driver.find_element_by_class_name("js-username-field")
    password_field = browser_driver.find_element_by_class_name("js-password-field")

    username_field.send_keys(the_login)
    #browser_driver.implicitly_wait(2)

    time.sleep(2)

    password_field.send_keys(the_password)
    #browser_driver.implicitly_wait(2)

    browser_driver.find_element_by_class_name("EdgeButtom--medium").click()

    time.sleep(3)

    followers_names = list()

    body = browser_driver.find_element_by_tag_name("body")

    while how_many_followers > 0:
        found_name = browser_driver.find_element_by_class_name("css-901oao.css-bfa6kz.r-111h2gw.r-18u37iz.r-1qd0xha.r-a023e6.r-16dba41.r-ad9z0x.r-bcqeeo.r-qvutc0")
        if found_name.text not in followers_names:
            followers_names.append(found_name.text)
            how_many_followers -= 1
        body.send_keys(Keys.PAGE_DOWN)
        time.sleep(0.5)

    print(followers_names)

    with open("followers_names.csv", "w", newline='') as myfile:
        wr = csv.writer(myfile, quoting=csv.QUOTE_ALL, delimiter="\n")
        wr.writerow(followers_names)

    browser_driver.quit()
    sys.exit(1)


user_followers(path_arg, user_name_arg, login_arg, password_arg, followers_arg)

#python GetUserFollowers.py "C:\Users\ppmic\Desktop\chromedriver.exe" "Marvel" "mkskipper@gmail.com" "123qazxsw123" 50

