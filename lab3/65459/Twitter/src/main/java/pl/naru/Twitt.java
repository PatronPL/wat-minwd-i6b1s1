package pl.naru;

import twitter4j.*;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.*;

public class Twitt {
    public static void main(String[] args) throws TwitterException {
        Twitter twitter = TwitterFactory.getSingleton();
        List<Status> statuses = twitter.getHomeTimeline();
        List<Status> dayStatuses = new ArrayList<>();
        System.out.println("-----------------------------------------------------------");

        Scanner sc = new Scanner(System.in);
        System.out.println("Type in name of the user without " + "\" @ \" ");
        String name = sc.nextLine();

        fetchTweets("@" + name, twitter, statuses);
        User user = twitter.showUser("@" + name);

        System.out.println("======================================================");
        System.out.println("User info:");
        System.out.println("Name: " + user.getName());
        System.out.println("Description: " + user.getDescription());
        System.out.println("Localisation: " + user.getLocation());
        System.out.println("Joined: " + user.getCreatedAt());
        System.out.println("======================================================");

        System.out.println("First Tweet: ");
        tweetWriter(statuses.get(0));
        System.out.println("Last Tweet: ");
        tweetWriter(statuses.get(statuses.size() - 1));
        System.out.println("----------------------------------------------");
        System.out.println("----------------------------------------------");
        System.out.println("To get word count from X days");
        System.out.println("Type Date 'yyyy-MM-dd':");
        String dateS = sc.nextLine();

        SimpleDateFormat sdformat = new SimpleDateFormat("yyyy-MM-dd");
        Date date = null;
        try {
            date = sdformat.parse(dateS);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        tweetInDays(statuses, date, dayStatuses);
        System.out.println("All Tweets: " + statuses.size());
        System.out.println("Tweets in that period of time: " + dayStatuses.size());
        StringBuilder sb = new StringBuilder();
        for(Status s : dayStatuses){
           sb.append(s.getText());
        }
        String test = sb.toString();

        test = test.replace(",", "");
        test = test.replace(".", "");

        String[] preparedArray = test.split(" ");
        Map<String, Integer> theCloud = new HashMap<>();

        for (String s : preparedArray) {
            if (!theCloud.containsKey(s)) theCloud.put(s, 1);
            else theCloud.replace(s, theCloud.get(s) + 1);
        }
        List<Map.Entry<String, Integer>> list = new ArrayList<>(theCloud.entrySet());
        list.sort(Map.Entry.comparingByValue());
        System.out.println("Press any key to show Word cloud");
        sc.nextLine();
        for (Map.Entry<String, Integer> k : list) {
            System.out.println(k.getKey()+"       =  "+k.getValue());
        }
    }

    public static void tweetInDays(List<Status> statuses, Date since, List<Status> dayStatuses) {
        for (Status status : statuses) {
            if (status.getCreatedAt().after(since)) {
                dayStatuses.add(status);
            } else if (status.getCreatedAt().equals(since)) {
                dayStatuses.add(status);
            } else if (status.getCreatedAt().before(since)) {
                break;
            }
        }
    }

    public static void tweetWriter(Status status) {
        System.out.println("###############");
        System.out.println("Post Date: " + status.getCreatedAt());
        System.out.println("Text: " + status.getText());
        System.out.println("Favorite count: " + status.getFavoriteCount());
        System.out.println("########################################################");
    }

    public static void fetchTweets(String handle, Twitter twitter, List<Status> statuses) throws TwitterException {
        statuses.clear();
        Paging page = new Paging(1, 320);
        int p = 1;

        while (p <= 10) {
            page.setPage(p);
            statuses.addAll(twitter.getUserTimeline(handle, page));
            p++;
        }
    }
}
