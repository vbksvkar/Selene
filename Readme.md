# Selene
A Reddit scraper to crawl posts and gather statistics.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Installation](#installation)
4. [Usage](#usage)
5. [Configuration](#configuration)
6. [Contributing](#contributing)

---

## Introduction
Selene uses APIs provided by Reddit to query sub-reddits and perform statistical analysis of the posts. At any point in time, the application will pull - 
- Posts with most upvotes
- Users with most posts
- Most cross-posted posts
Selene also provides an API to get the statistics through a simple RESTful endpoint.

## Features
- **Worker**: Background service which runs at a configurable interval to get the statistics of the sub-reddits
- **API**: A RESTful GET Api to provide the statistics

## Installation

Instructions on how to install the application.

### Prerequisites

- **Required Software**: .NET 8
- **Operating Systems**: Windows, Linux, macOS
- **Secrets Manager**: 

### Steps

1. Clone the repository:
    ```bash
    git clone https://github.com/vbksvkar/Selene.git
    ```
2. Navigate to the folder:
    ```bash
    cd Selene
    ```    
3. Install dependencies:
    ```bash
    dotnet restore
    ```
4. Build the application:
    ```bash
    dotnet build
    ```
5. Configure Secrets Manager:
    This application uses ```user secrets``` feature of .NET to store the secret information.

    The Reddit's access_token API uses Auth Type of Basic Auth with Username as clientId and Password as clientsecret.    
    
    - Install Secrets Manager:
       If you don't have the Secret Manager tool already installed, you can install it with the below command - 
       ```bash
       dotnet tool install --global dotnet-user-secrets
       ```
    - Add clientId to the secrets manager
        ```bash
        dotnet user-secrets set "clientId" "<value of the clientId sent in the email>"
        ```
    - Add clientSecret to the secrets manager
        ```bash
        dotnet user-secrets set "clientSecret" "<value of the clientSecret sent in the email>"
        ```
    <sup>```The values of clientId and clientSecret are shared through email.```</sup>

6. Start the application:
    ```bash
    dotnet run --project Api/Api.csproj
    ```

## Usage

When the application starts, the worker process starts pulling the statistics on a configurable interval.

#### The Worker

For Authentication:
- Authenticates with the Reddit API
- Stores the token in memory after the first call to access_token call.
- When the token expiry is less than 60 secs, it grabs the new token and updates the in-memory token

For Rate Limiting:
- Gets the rate limiting headers after the first call
- If the remaining requests is less than 1, then it pauses for reset header sent in the response of the call to the Reddit API

#### The API
The API is configured to expose the http endpoints on port ```35600```. This ensures that none of the applications conflict with the port of the already running applications (if any).

GET ```http://localhost:35600/api/reddit?subRedditName=r/askreddit```
- Query Params:
    - subRedditName: Name of the subreddit to get the statistics

- Response: (Reduced for brevity)
    
    Lists out 3 statistics: Posts with most up-votes, Users with most posts, most cross-postings

    ```json
    {
        "subredditName": "r/AskReddit",
        "statsDetails": [
        {
            "statsText": "Posts with most up-votes this month",
            "statsItems": [
            {
                "statValue": "Guys with extremely loud vehicles ...",
                "count": 20288,
                "statCount": "20.3K"
            }]
        },
        {
            "statsText": "Users with most posts this month",
            "statsItems": [{
                "statValue": "JollySimple188",
                "count": 3,
                "statCount": "3"
            }]
        },
        {
            "statsText": "Posts with most cross postings this month",
            "statsItems": [{
                "statValue": "What’s a crazy body life hack everyone should know?",
                "count": 17,
                "statCount": "17"
            }]
        }]
    }
    ```

## Configuration

The following are the list of configurable items for the application - 
- subRedditConfig: Is a list of config objects with 
    - name: Name of the subreddit to use while query the Reddit API
    - interval: valid values of ```hour, day, week, month, year, all```
    - postsCount: the limiting count of posts to query from the Reddit API
- workerInterval: The interval at which worker will call the reddit apis to get the statistics for the posts. 


## Contributing

If you'd like others to contribute to your project, add guidelines here.

```
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Commit your changes (`git commit -m "Add feature"`).
4. Push to the branch (`git push origin feature-branch`).
5. Create a pull request.
```
