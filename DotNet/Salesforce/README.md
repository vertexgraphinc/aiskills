# Salesforce Skill

This skill allows the authenticated user to perform simple tasks on his/her Salesforce account directly from the VertexGraph chat prompt.

## API Permissions Setup

1. Go to <https://login.salesforce.com>
2. Login with your Salesforce developer account credentials.
3. Go to **Gear Icon > Setup.**

![Salesforce Setup Navigation Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Salesforce/images/Salesforce_Setup_Navigation.png)

4. Under **Platform Tools**, navigate to **Apps > App Manager**. Select **New Connected App**.

![Application Basic Info Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Salesforce/images/Salesforce_New_Connected_App_Navigation.png)

5. Fill out the form with all required information marked in red. Under the **API (Enable OAuth Settings)** section, choose exactly the following setup option in the image below with the scopes (_api, offline_access, refresh_token_). Then add the following links into the **Callback URL** list:

<https://api.vertexgraph.com/adminapi/assets/oauthcode>

<http://localhost/salesforce/auth> (only apply to local setup, public url is different)

<http://localhost/salesforce/token> (only apply to local setup, public url is different)

![OAuth Setup Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Salesforce/images/Salesforce_OAuth_Setup.png)

6. After filling out the information, click **Save** to create the connected app. Then navigate to **Apps > App Manager** and scroll down the list to identify your newly created app. Click on the arrow icon at the end then click **View**.

![View App Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Salesforce/images/Salesforce_View_App.png)

7. Next to **Consumer Key and Seret**, click on **Manage Consumer Details**. It might require you to verify yourself with a verification code. After completing it, following screen as in the image below will pop up. Copy down the **Consumer Key (Client ID)** and **Consumer Secret (Client Secret)** for authorization.

![Consumer Detail Screenshot](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Salesforce/images/Salesforce_Consumer_Details.png)

(*) Note: There is a special condition for Salesforce skill to work, which is that the API needs to use the users' org/personal domain which can be found in the Salesforce Setup page at **Settings > Company Settings > My Domain** and then use it to update the **SalesforceApiBaseURL** constant inside the code base (only change the domain/subdomain, the path stays the same).

## Test Prompts

Please refer to the root README (https://github.com/vertexgraphinc/aiskills/blob/main/DotNet/README.md) for instructions on how to host the custom skill and install it on the VertexGraph.ai website. Once the Salesforce Skill is properly installed, you can try the following example prompts on the AI Assistant:

- Get all the campaigns from Salesforce happening from tomorrow
- Create a campaign named "Test campaign" from today till Monday next week
- Update the campaign named "Test campaign" with the new description "updated description"
- Remove all campaigns happening after today
- Get all contacts with the first name John
- Create a contact for John Doe with email example@example.com and phone number 1234567890
- Update John Doe contact email to update@example.com
- Remove all contacts with first name John