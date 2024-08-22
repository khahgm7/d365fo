##ISSUETITLE

Unable to login with new Warehouse management App changes

##SYMPTOM

We are unable to login with new Warehouse management App changes, because of this error:
 
"AADSTS700016: Application with identifier was not found in the directory. This can happen if the application has not been installed by the administrator of the tenant or consented to by any user in the tenant. You may have sent your authentication request to the wrong tenant"
 
##CAUSE
 
Incorrect setup

##RESOLUTION
 
As it turned out, it was needed to grant admin consent in the tenant, as per related documentation: https://learn.microsoft.com/en-us/dynamics365/supply-chain/warehousing/warehouse-app-authenticate-user-based#create-service.

On a different note, I realize that switching to new authentication method may take time.

But older versions of the mobile app will continue to work and will continue to support service-based authentication, even after version 3.0 is released: https://learn.microsoft.com/en-us/dynamics365/supply-chain/warehousing/warehouse-app-user-based-auth-faq#which-version-of-the-mobile-app-will-service-based-authentication-be-removed-in.

Any mobile app version below version 3.0 can still be used for service-based authentication:

“As of version 3.0, the Warehouse Management mobile app will no longer support service-based authentication. Therefore, existing certificates and client secrets will no longer work.

However, we strongly recommend that you switch to user-based authentication as soon as possible. Devices that are set to automatically update apps from app stores (such as Microsoft Store, Google Play, or Apple App Store) will automatically get the latest version of the mobile app…”

As a side note, as of now, version 3.0 is not yet released.
 
So, if you don’t automatically update apps from app stores, you will not get version 3.0 once it gets released.

But obviously, staying on an older version of the mobile app means that you will not benefit from changes from newer versions.
 
So, please make changes to authentication method as soon as possible.

For a comprehensive understanding of this transition, we highly recommend reading through our FAQ, which can be found at our official Microsoft Learn website https://learn.microsoft.com/en-us/dynamics365/supply-chain/warehousing/warehouse-app-user-based-auth-faq.
 
Additionally, for those who favor visual learning, a detailed 28-minute video https://youtu.be/4K17gF5msv0 is available, providing a thorough explanation of the new authentication process and its benefits.
 
 
For further assistance, please refer to the documentations provided in the links:
 
- The full documentation https://learn.microsoft.com/en-us/dynamics365/supply-chain/warehousing/warehouse-app-authenticate-user-based
 
- MDM: https://learn.microsoft.com/en-us/dynamics365/supply-chain/warehousing/warehouse-app-intune-user-based
 
- On-prem: https://learn.microsoft.com/en-us/dynamics365/fin-ops-core/dev-itpro/deployment/warehousing-onprem-userauth
 
 
As alternative source of information, please check also this LinkedIn post (please note that this is not an official Microsoft document, so please use it as it is, without any guarantee that included information fully follows official documentation):
 
https://www.linkedin.com/pulse/user-based-authentication-device-code-flow-d365-app-shoman--vxgff/?trackingId=VLTHJN5tM3xOJqVHxrL9IA%3D%3D