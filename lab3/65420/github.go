package main

import (
	"context"
	"fmt"
	"net/http"
	"strconv"

	"github.com/go-echarts/go-echarts/charts"
	"github.com/google/go-github/github"
	"golang.org/x/oauth2"
	githuboauth "golang.org/x/oauth2/github"
)

var (
	// You must register the app at https://github.com/settings/applications
	// Set callback to http://127.0.0.1:7000/github_oauth_cb
	// Set ClientId and ClientSecret to
	oauthConf = &oauth2.Config{
		ClientID:     "b763f3b41be5230968e7",
		ClientSecret: "7c3b383ca3cd07960f331d01fa5c18c0397fe237",
		// select level of access you want https://developer.github.com/v3/oauth/#scopes
		Scopes:   []string{"repo", "user:follow"},
		Endpoint: githuboauth.Endpoint,
	}
	// random string for oauth2 API calls to protect against CSRF
	oauthStateString = "hispalmsspaghettikneesweakarmsspaghetti"
)

const htmlBegin = `<!DOCTYPE html>
<html>
<head>
<style>
@import url(https://fonts.googleapis.com/css?family=Cabin:400);

.webdesigntuts-workshop {
	background: #151515;
	min-height: 100%;
	min-width: 1024px;
	width: 100%;
	height: 3200px
	position: fixed;
	top: 0;
	left: 0;
}


.webdesigntuts-workshop form {
	background: #111;
	background: linear-gradient(#1b1b1b, #111);
	border: 1px solid #000;
	border-radius: 5px;
	box-shadow: inset 0 0 0 1px #272727;
	display: inline-block;
	font-size: 0px;
	padding: 20px;
	z-index: 1;
	margin: 0;
	position: absolute;
	top: 40%;                    
    left: 50%;
    margin-right: -50%;
	transform: translate(-50%,-50%)
}

.webdesigntuts-workshop input {
	background: #222;	
	background: linear-gradient(#333, #222);	
	border: 1px solid #444;
	border-radius: 5px 0 0 5px;
	box-shadow: 0 2px 0 #000;
	color: #888;
	display: block;
	float: left;
	font-family: 'Cabin', helvetica, arial, sans-serif;
	font-size: 13px;
	font-weight: 400;
	height: 40px;
	margin: 0;
	padding: 0 10px;
	text-shadow: 0 -1px 0 #000;
	width: 200px;
}



.ie .webdesigntuts-workshop input {
	line-height: 40px;
}

.webdesigntuts-workshop input::-webkit-input-placeholder {
   color: #888;
}

.webdesigntuts-workshop input:-moz-placeholder {
   color: #888;
}

.webdesigntuts-workshop input:focus {
	animation: glow 800ms ease-out infinite alternate;
	background: #222922;
	background: linear-gradient(#333933, #222922);
	border-color: #393;
	box-shadow: 0 0 5px rgba(0,255,0,.2), inset 0 0 5px rgba(0,255,0,.1), 0 2px 0 #000;
	color: #efe;
	outline: none;
}

.webdesigntuts-workshop input:focus::-webkit-input-placeholder { 
	color: #efe;
}

.webdesigntuts-workshop input:focus:-moz-placeholder {
	color: #efe;
}

.webdesigntuts-workshop button {
	background: #222;
	background: linear-gradient(#333, #222);
	box-sizing: border-box;
	border: 1px solid #444;
	border-left-color: #000;
	border-radius: 0 5px 5px 0;
	box-shadow: 0 2px 0 #000;
	color: #fff;
	display: block;
	float: left;
	font-family: 'Cabin', helvetica, arial, sans-serif;
	font-size: 13px;
	font-weight: 400;
	height: 40px;
	line-height: 40px;
	margin: 0;
	padding: 0;
	position: relative;
	text-shadow: 0 -1px 0 #000;
	width: 80px;
}	

.webdesigntuts-workshop button:hover,
.webdesigntuts-workshop button:focus {
	background: #292929;
	background: linear-gradient(#393939, #292929);
	color: #5f5;
	outline: none;
}

.webdesigntuts-workshop button:active {
	background: #292929;
	background: linear-gradient(#393939, #292929);
	box-shadow: 0 1px 0 #000, inset 1px 0 1px #222;
	top: 1px;
}

h1.nickname {
	font-size: 500%;
	left: 0;
	width: 100%;
	text-align: center;
}

h2.repo {
	font-size: 300%;
	left: 0;
	width: 100%;
	text-align: center;
}



#repcontainer ul
{
	font-size: 150%;
	left: 0;
	width: 100%;
	text-align: center;
}



@keyframes glow {
    0% {
		border-color: #393;
		box-shadow: 0 0 5px rgba(0,255,0,.2), inset 0 0 5px rgba(0,255,0,.1), 0 2px 0 #000;
    }	
    100% {
		border-color: #6f6;
		box-shadow: 0 0 20px rgba(0,255,0,.6), inset 0 0 10px rgba(0,255,0,.4), 0 2px 0 #000;
    }
}
</style>
</head>
<body>

<section class="webdesigntuts-workshop">`

const htmlEnd = `</section>
</body>
</html>`

var githubUser string
var githubRepositiories []string
var followersDeep int
var data Data
var githubUsers []string
var flag bool = false

//Data for graph of followers
type Data struct {
	Nodes []charts.GraphNode
	Links []charts.GraphLink
}

func handleMain(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "text/html; charset=utf-8")
	w.WriteHeader(http.StatusOK)
	w.Write([]byte(htmlBegin))
	if !flag {
		flag = !flag
		fmt.Fprintf(w, "<form action='/input' method='put'><input type='text' name='user' placeholder='Nazwa użytkownika'><input type='number' name='graph' placeholder='Liczba poziomów grafu'><button>Szukaj</button></form></section>")
		w.Write([]byte(htmlEnd))

	} else {
		w.Write([]byte(htmlEnd))
	}
}

func handleMainInput(w http.ResponseWriter, r *http.Request) {
	r.ParseForm()
	githubUser = r.FormValue("user")
	deep, err := strconv.Atoi(r.FormValue("graph"))
	if err != nil {
	}
	followersDeep = deep
	fmt.Printf("User: %s, Deep: %d\n", githubUser, followersDeep)

	http.Redirect(w, r, "/login", http.StatusTemporaryRedirect)
}

func handleGitHubLogin(w http.ResponseWriter, r *http.Request) {
	url := oauthConf.AuthCodeURL(oauthStateString, oauth2.AccessTypeOnline)
	http.Redirect(w, r, url, http.StatusTemporaryRedirect)
}

func handleGitHubCallback(w http.ResponseWriter, r *http.Request) {
	state := r.FormValue("state")
	if state != oauthStateString {
		fmt.Printf("invalid oauth state, expected '%s', got '%s'\n", oauthStateString, state)
		http.Redirect(w, r, "/", http.StatusTemporaryRedirect)
		return
	}

	code := r.FormValue("code")
	token, err := oauthConf.Exchange(oauth2.NoContext, code)
	if err != nil {
	}

	oauthClient := oauthConf.Client(oauth2.NoContext, token)
	client := github.NewClient(oauthClient)
	user, _, err := client.Users.Get(context.Background(), "")
	if err != nil {
		fmt.Printf("client.Users.Get() faled with '%s'\n", err)
		http.Redirect(w, r, "/", http.StatusTemporaryRedirect)
		return
	}
	fmt.Printf("Logged in as GitHub user: %s\n", *user.Login)

	fmt.Println("Looking for repositories")
	repos, _, err := client.Repositories.List(context.Background(), githubUser, nil)
	if err != nil {
		fmt.Printf("client.Repositories.ListAllTopics) failed with '%s'\n", err)
		http.Redirect(w, r, "/", http.StatusTemporaryRedirect)
		return
	}

	for _, repo := range repos {
		githubRepositiories = append(githubRepositiories, repo.GetName())
	}

	fmt.Println("Looking for followers")

	followers, _, err := client.Users.ListFollowers(context.Background(), githubUser, nil)
	if err != nil {
		fmt.Printf("client.Users.ListFollowers()) failed with '%s'\n", err)
		http.Redirect(w, r, "/", http.StatusTemporaryRedirect)
		return
	}
	x := float32(-1000000) * float32(len(followers)/2)
	y := float32(-1000000) * float32(len(followers))

	data.Nodes = append(data.Nodes, charts.GraphNode{SymbolSize: 10, Name: githubUser, X: x, Y: y})
	githubUsers = append(githubUsers, githubUser)
	getFollowers(w, r, client, githubUser, x, y)

	http.Redirect(w, r, "/github_user", http.StatusTemporaryRedirect)
}

func handleGitHubUser(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "text/html; charset=utf-8")
	w.WriteHeader(http.StatusOK)
	w.Write([]byte(htmlBegin))

	fmt.Fprintf(w, "<h1 class='nickname'>Użytkownik: %s</h1>\n<h2 class='repo'>Repozytoria:</h2>\n<div id='repcontainer'>\n<ul>\n", githubUser)
	for _, repository := range githubRepositiories {
		fmt.Fprintf(w, "<li>%s</li>\n", repository)
	}
	fmt.Fprintf(w, "</ul>\n</div><h2 class='repo'>Graf:</h2>\n")
	graph(w)
	w.Write([]byte(htmlEnd))
	flag = !flag
}

func getFollowers(w http.ResponseWriter, r *http.Request, client *github.Client, userLogin string, _x float32, _y float32) {
	followers, _, err := client.Users.ListFollowers(context.Background(), userLogin, nil)
	if err != nil {
		fmt.Printf("client.Users.ListFollowers()) failed with '%s'\n", err)
		http.Redirect(w, r, "/", http.StatusTemporaryRedirect)
		return
	}

	var followersList []string
	for _, follower := range followers {
		if contains(githubUsers, follower.GetLogin()) == false {
			githubUsers = append(githubUsers, follower.GetLogin())
			followersList = append(followersList, follower.GetLogin())
		} else {
			data.Links = append(data.Links, charts.GraphLink{Source: follower.GetLogin(), Target: userLogin})
		}
	}

	var y float32 = _y + 500000
	var lowx float32 = _x / float32(len(followersList))
	var upx float32 = lowx * float32(2)
	for j, follower := range followersList {
		x := lowx + upx*float32(j)
		data.Links = append(data.Links, charts.GraphLink{Source: follower, Target: userLogin})
		if followersDeep > 0 {
			data.Nodes = append(data.Nodes, charts.GraphNode{SymbolSize: 10, Name: follower, X: x, Y: y})
			followersDeep--
			getFollowers(w, r, client, follower, x, y)
			followersDeep++
		}
	}
}

func contains(logins []string, login string) bool {
	for _, existing := range logins {
		if existing == login {
			return true
		}
	}
	return false
}

func graph(w http.ResponseWriter) {
	graph := charts.NewGraph()
	graph.SetGlobalOptions(charts.TitleOpts{Title: "Github"})

	graph.Add("Github user", data.Nodes, data.Links,
		charts.GraphOpts{Layout: "none", Roam: true, FocusNodeAdjacency: true},
		charts.EmphasisOpts{Label: charts.LabelTextOpts{Show: true, Position: "left", Color: "black"}},
		charts.LineStyleOpts{Curveness: 0.1},
	)

	graph.Render(w)
	return
}

func main() {
	//	githubUser = "thatrobotdev"
	//	followersDeep = 2
	http.HandleFunc("/", handleMain)
	http.HandleFunc("/input", handleMainInput)
	http.HandleFunc("/login", handleGitHubLogin)
	http.HandleFunc("/github_oauth_cb", handleGitHubCallback)
	http.HandleFunc("/github_user", handleGitHubUser)
	fmt.Print("Started running on http://127.0.0.1:7000\n")
	fmt.Println(http.ListenAndServe(":7000", nil))

}
