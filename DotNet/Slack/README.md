# Slack Skill

## Creating Slack App:

1. Go to <https://api.slack.com/apps>
2. Click on Create New app

![Create New App Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Create_New_App.png)

3. Select “From Scratch”

![Create an App from Scratch Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Create_an_App_from_Scratch.png)

4. For App Name type “Slack AI Skill” and then select which workspace you would like to install it to.
5. Once that is done, navigate to the OAuth & Permissions Tab on the left side bar.

![OAuth and Permissions Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/OAuth_and_Permissions.png)

6. Scroll down to Redirect URL section and add the following URL’s.

<https://api.vertexgraph.com/adminapi/assets/oauthcode>

<https://localhost/slack/oauth/auth>

<https://localhost/slack/oauth/token>

![Redirect URLs Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Redirect_URLs.png)

7. Once that is done, navigate to the Basic Information Tab on the left side bar.

![Basic Information Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Basic_Information.png)

8. Scroll down and you will see your App Credentials, you want to copy your **Client ID** and **Client Secret**

## Setting up Skill in VertexGraph

1. Navigate to <https://vertexgraph.ai/portal/>
2. On the left side tool bar select the second option “AI”
3. Now select the custom skills icon on the top right corner

![AI Assistants Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/AI_Assistants.png)

1. Once on the skills section Press “New Custom Skill”
2. Follow the setup until you reach the section where you must enter the OAuth Client ID and Secret
3. Copy and paste the client ID and secret from the Slack AI Skill information
4. Get the authorization code and authorize VertexGraph to access your slack account
5. Once completed assign the new skill to any of your assistants to start using it

## Test Prompts:

- Set a reminder to meet with John at 10am tomorrow
- Set my status on slack to be “Going on lunch break” till 1pm
- Get all of my slack direct message new activity
- Get all of my slack group messages
- Get new activity from all slack channels
- Search in slack for meeting documents
- Search in slack for all messages with “deploy”
- Set dnd for 2 hours
- Get new messages from general channel
- Send a message to John Doe saying Hello
- Send a slack message in the general channel saying Hello