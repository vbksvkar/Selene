# Selene - A Reddit scraper application
A simple Reddit scraper to crawl posts and gather statistics.
<br />
Provides a way to find the following statistics of sub-reddits - <br />
- Posts with most upvotes
- Users with most posts
- Most cross-posted posts

A configuration item is provided containing a list of subreddit names. Add/Update the subreddit names and get the desired statistics for each of the configured subreddit.

### Worker
A background service starts when the application starts to get the above listed statistics from Reddit for the list of subreddits configured in appsettings. The background service runs at a configurable frequency and collects the posts based on the configurable interval (month, hour, day, all etc.). 
The frequency to get the posts is configurable with ```WorkerInterval``` configurable item.

### Api
A WebApi solution is provided to query the stats for a given subreddit name.
URL to hit the Api is /api/reddit?subRedditName=<name of the subreddit>
<br /> for e.g. /api/reddit?subRedditName=r/funny

### User Secrets
This application uses ```user secrets``` feature of .NET to store the secret information.
The Reddit's access_token API uses Auth Type of Basic Auth with Username as clientId and Password as clientsecret.
The user secrets used in this application stores the information of clientId and clientSecret.
The values of clientId and clientSecret are shared through email. 

#### Install ```dotnet-user-secrets```
If you don't have the Secret Manager tool already installed, you can install it with the below command - 
```bash
dotnet tool install --global dotnet-user-secrets
```

#### Adding the clientId and clientSecret to the Secret Manager
After downloading the code navigate to the Api folder and use the following commands to add the secrets - 
```bash
dotnet user-secrets set "clientId" "<value of the clientId sent in the email"
```
```bash
dotnet user-secrets set "clientSecret" "<value of the clientSecret send in the email"
```
In the code the values of clientId and clientSecret is used by implementing the options pattern.

