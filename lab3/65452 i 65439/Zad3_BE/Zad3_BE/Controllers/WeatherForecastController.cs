using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Zad3_BE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {
        private static TwitterCredentials creds;
        public TwitterController()
        {
            creds = new TwitterCredentials("6fLuXUh0enly5O6imWzeLgKfI", "ogZbigHWoGaswkkbTEYAGUf6wFRNYUZppFVwyOSTGPZvKykbdK",
               "2513271864-nbWly1iXCZwiBs65VTz4WyY4IaV6Yz2FKDivEOv", "tpfbKnt099xYxs6Qv0eSzunNNuvR8KQw5Sumjt8aCSjLm");
        }

        [HttpGet("user-info/{screenName}")]
        public async Task<IActionResult> GetUser(string screenName)
        {
            var user = await Auth.ExecuteOperationWithCredentials(creds, () =>
            {
                return UserAsync.GetUserFromScreenName(screenName);
            });

            return Ok(user);
        }

        [HttpGet("first-and-last/{screenName}")]
        public async Task<IActionResult> GetFirstAndLastAsync(string screenName)
        {
            var userIdentifier = new UserIdentifier(screenName);

            var tweets = await GetTweets(screenName);

            var orderedTweets = tweets.OrderBy(x => x.CreatedAt);

            ITweetDTO[] response = new ITweetDTO[] { orderedTweets.FirstOrDefault(), orderedTweets.LastOrDefault() };

            return Ok(response);
        }

        [HttpGet("top-words/{screenName}")]
        public async Task<IActionResult> GetToWordsAsync(string screenName, [FromQuery] DateTime fromDate)
        {
            var takeTop = 30;

            var tweets = await GetTweets(screenName);

            var filteredTweets = tweets.Where(x => fromDate.CompareTo(x.CreatedAt) <= 0);

            var response = new List<string>();

            var words = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

            foreach (var tweet in filteredTweets)
            {
                var text = Regex.Replace(tweet.Text, @"\t|\n|\r|amp;|RT", "");

                foreach (var word in text.Split(' '))
                {
                    words.TryGetValue(word, out int currentCount);

                    currentCount++;
                    words[word] = currentCount;
                }
            }

            words.Remove("");

            var xx = words.OrderByDescending(x => x.Value).Take(takeTop);

            return Ok(xx);
        }

        public async Task<List<ITweetDTO>> GetTweets(string screenName)
        {
            var tweets = new List<ITweetDTO>();
            long maxId = 0;
            while (true)
            {
                var newBatch = await Auth.ExecuteOperationWithCredentials(creds, () =>
                {
                    var uri = $"https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={screenName}&count=200";
                    if (maxId != 0)
                    {
                        uri += $"&max_id={maxId}";
                    }
                    return TwitterAccessorAsync.ExecuteGETQuery<IEnumerable<ITweetDTO>>(uri);
                });
                if (newBatch.Count() == 0)
                {
                    break;
                }
                else
                {
                    tweets.AddRange(newBatch);
                    maxId = newBatch.LastOrDefault().Id - 1;
                }
            }
            return tweets;
        }
    }
}
