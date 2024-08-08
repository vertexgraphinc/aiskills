# Zoho Skill

This skill allows the authenticated user to perform simple tasks on his/her Zoho account directly from the VertexGraph chat prompt.

## Zoho App Setup

1. Go to <https://api-console.zoho.com/> to register the app in the Zoho's Developer Console.
2. Click on "Add Client".

   ![Add Client](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoho/Images/AddClient.PNG)


3. Choose client type as "Server-based Applications".

   ![Choose Client Type](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoho/Images/ChooseClientType.PNG)


4. Specify a name for the new.
5. Enter the Homepage URL: https://api.vertexgraph.com																
6. In the Authorized Redirect URIs enter: https://api.vertexgraph.com/adminapi/assets/oauthcode
7. Click on "Create".

   ![Create New Client](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoho/Images/CreateNewClient.PNG)


8. The Client ID and Client Secret will be displayed. Copy these values to a secure place.

   ![Client Secret](https://raw.githubusercontent.com/vertexgraphinc/aiskills/main/DotNet/Zoho/Images/ClientSecret.PNG)


9. Can close Zoho page now. With the saved values, you can now authenticate the user and access the Zoho APIs.

 
Please refer to the root README (https://github.com/vertexgraphinc/aiskills/blob/main/DotNet/README.md) for instructions on how to host the custom skill and install it on the VertexGraph.ai website.

