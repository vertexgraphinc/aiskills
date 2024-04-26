# Jira Skill

This skill allows the authenticated user to perform simple tasks on his/her Atlassian account directly from the VertexGraph chat prompt.

## Jira App Setup

1. Go to <https://developer.atlassian.com/console/myapps> 
2. Click on Create and then click OAuth 2.0 integration.
3. Add your app name and press create.

   ![Create New App Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/CreateNewApp.png)


4. Now head to the “Permissions” tab on the left

   ![PermissionsPage](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/PermissionsPage.png)

1. Click on the Add button by “Jira API” and once its added, press the button a second time when it says “Configure” to add scopes 

   ![AddPermissionsPage](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/AddPermissionsPage.png)

1. Now to edit scopes click the “Edit Scopes” button on the top right of the screen which will open a pop up screen

   ![SetPermissions](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/SetPermissions.png)

1. Now select the following scopes and press save

   ![EditPermissions](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/EditPermissions.png)

1. Once you have added your scopes successfully now click on “Authorization” in the left sidebar and then click add on “OAuth 2.0 (3LO)”

   ![AddCallback](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/AddCallback.png)

9. On this screen you need to add the following callback URL and press save: https://api.vertexgraph.com/adminapi/assets/oauthcode

   ![AddCallback2](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/AddCallback2.png)


1. Now click on Settings on the left sidebar scroll down and you will see your “Authentication details” you want to copy your **Client ID** and **Secret**

   ![GetAppInfo](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/GetAppInfo.png)

1. You may now try to setup the skill on VertexGraph. If you end up coming across this page saying you do not have access to the app please complete the following setups

   ![NoAccess](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/NoAccess.png)

1. Return back to https://developer.atlassian.com/console/myapps and click on your app. Then click on Distribution on the left sidebar and fill out the following information accordingly.

   ![Distribution](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Jira/Images/Distribution.png)

13.	Once filled out, press save changes at the bottom to set your app distribution to sharing. Now you may return to step 1 of setting up the skill in VertexGraph to try again. 


Please refer to the root README (https://github.com/vertexgraphinc/aiskills/blob/main/DotNet/README.md) for instructions on how to host the custom skill and install it on the VertexGraph.ai website.

