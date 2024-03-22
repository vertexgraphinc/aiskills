# Slack Skill

# VertexGraph AI Skill – Slack

## Creating a Slack App:

1. Go to https://api.slack.com/apps
2. Click on Create New app
   ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Create_New_App.png)
3. Select “From Scratch”
   ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Create_an_App.png)
4. For App Name type “Slack AI Skill” and then select which workspace you would like to install it to.
   ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Name_App_and_Choose_Workspace.png)
5. Once that is done, you will be navigated to the Basic Information page of the app automatically
   ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Basic_Information.png)
6. Now navigate to the OAuth & Permissions Tab on the left side bar.
   ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/OAuth_and_Permissions.png)
7. Scroll down to Redirect URL section and add the following URL’s.
   https://api.vertexgraph.com/adminapi/assets/oauthcode
   https://localhost/slack/oauth/auth
   https://localhost/slack/oauth/token
   ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Redirect_URLs.png)
8. Once that is done, navigate back to the Basic Information Tab on the left side bar.
   ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Return_to_Basic_Information.png)
9. Scroll down and you will see your App Credentials, you want to copy your Client ID and Client Secret
   ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/App_Credentials.png)
10. Now you are done with creating and setting up the slack app, you don’t need to do anything else and you can now go setup the slack skill in VertexGraph
    Setting up Skill in VertexGraph
11. Navigate to https://vertexgraph.ai/portal/
12. On the left side tool bar select the second option “AI”
13. Now select the custom skills icon on the top right corner
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/AI_Assistants.png)
14. Once on the skills section Press “New Custom Skill”
15. Scroll to the bottom and select Desktop as your knowledge source type
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Knowledge_Source_Type.png)
16. Press Continue
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Name.png)
17. Type http://localhost/slack in the first box and press continue
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/skill_URL.png)
18. Copy and paste your client id and secret which you got from the steps above into the input fields and continue
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Client_ID_and_Secret.png)
19. On the next screen click on “Get Code”
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Authorization_Screen.png)
20. This will open a pop-up screen asking you to sign in to your workspace.
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Slack_Sign_In.png)
21. Follow the steps until you see this screen
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/OAuth_Permissions_Popup.png)
22. Scroll all the way down and click “Allow”
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Allow_Screen.png)
23. Once pressing allow you will be redirected back to VertexGraph with the code. Press continue.
    ![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Slack/images/Authorization_Screen_Filled_In.png)
24. You have now successfully set up the skill, to use it assign it to any of your assistants

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