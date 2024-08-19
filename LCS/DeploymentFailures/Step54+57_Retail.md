
## Solution

1. Connect to the intended environment.
2. Find the latest deployable package applied to the environment. It will be under folder <ServiceVolue>:\DeployablePackages\<PackageGUID>
3. Under the deployable package folder, find the following SQL script <ServiceVolue>:\DeployablePackages\<PackageGUID>\RetailServer\Scripts\DropAllRetialChannelDbObjects.sql
4. This script, when run against the AOS database, will remove all objects related to the Retail Channel Database. Only proceed if you are not using the Retail Channel functionality.
5. Connect to the AOS database and run the above script. This operation will take several minutes.