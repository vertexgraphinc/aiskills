# Gmail Skill

This skill allows the authenticated user to perform simple tasks on his/her Google Gmail account directly from the VertexGraph chat prompt.

## API Permissions Setup

 - Step 1: Go to the Google Cloud Console at https://console.cloud.google.com/apis/dashboard
 - Step 2: Sign in with the account that you want to expose to the skill
 - Step 3: On the left-side navigation, click on the "Enabled APIs & Services"
![Enabled APIs and Services Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GMail/images/Enabled_APIs_and_Services.png)

 - Step 4: Enable the Gmail API
![Enable the Gmail API Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GMail/images/Enable_the_Gmail_API.png)

 - Step 5: On the left side navigation. Click on Credentials. Create a new OAuth 2.0 Client ID
![Credentials Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GMail/images/Credentials.png)

 - Step 6: On the OAuth 2.0 Client ID details page, under Authorized JavaScript origins, add: https://api.vertexgraph.ai. Under the Authorized redirect URIs, add: https://api.vertexgraph.com/adminapi/assets/oauthcode
![Credentials URLs Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/GMail/images/Credentials_URLs.png)

## Test Prompts

Please refer to the root README (https://github.com/vertexgraphinc/aiskills/blob/main/DotNet/README.md) for instructions on how to host the custom skill and install it on the VertexGraph.ai website. Once the Google Gmail Skill is properly installed, you can try the following example prompts on the AI Assistant:

 - "How many emails have I received from support in the last 2 days?"
 - "Show me the full body of the last email from myfriend@example.com"
 - "Send an email to myself@example.com with subject 'hello world' and body 'this is a test email from the gmail skill'"
 - "Add a label called 'hello there' to the email containing the subject 'hello world'"
 - "Search for emails containing the label 'hello there'"
 - "Reply to the email from myfriend@example.com from today. Tell him that I approve the message"
 - "Show me a summary of the last 5 emails from support@example.com"
 - ...and many more
