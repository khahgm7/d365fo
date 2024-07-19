internal final class BTZ_Global_Security
{
    public static boolean canAccess(
        str _objectName
        , SecurityObjectType _type = SecurityObjectType::Privilege
        , UserId _userId = curUserId()
        )
    {
        boolean canAccess = isSystemAdministrator();
        
        if(!canAccess)
        {
            switch(_type)
            {
                case SecurityObjectType::Privilege:
                    canAccess = BTZ_Global_Security::canAccessWithPrivilege(_objectName, _userId);
                    break;
                case SecurityObjectType::Duty:
                    canAccess = BTZ_Global_Security::canAccessWithDuty(_objectName, _userId);
                    break;
                case SecurityObjectType::Role:
                    canAccess = BTZ_Global_Security::canAccessWithRole(_objectName, _userId);
                    break;
                default:
                    throw error(error::wrongUseOfFunction(funcName()));
            }
        }

        return canAccess;
    }

    public static boolean canAccessWithPrivilege(
        SecurityPrivilegeName _privilege
        , UserId _userId = curUserId()
        )
    {
        return BTZ_Global_Security::hasUserSecurityPrivilege(_privilege, _userId);
    }

    public static boolean canAccessWithDuty(
        SecurityDutyName _duty
        , UserId _userId = curUserId()
        )
    {
        return BTZ_Global_Security::hasUserSecurityDuty(_duty, _userId);
    }

    public static boolean canAccessWithRole(
        SecurityRoleName _role
        , UserId _userId = curUserId()
        )
    {
        return BTZ_Global_Security::hasUserSecurityRole(_role, _userId);
    }

    public static boolean hasUserSecurityPrivilege(
        SecurityPrivilegeName _privilege
        , UserId _userId = curUserId()
        )
    {
        SecurityRolePrivilegeExplodedGraph securityRolePrivilegeExplodedGraph;
        SecurityUserRole securityUserRole;
        SecurityPrivilege securityPrivilege;

        select firstonly securityPrivilege
            where securityPrivilege.Identifier == _privilege
        exists join securityRolePrivilegeExplodedGraph
            where securityRolePrivilegeExplodedGraph.SecurityPrivilege == securityPrivilege.RecId
        exists join securityUserRole
            where securityUserRole.SecurityRole == securityRolePrivilegeExplodedGraph.SecurityRole
               && securityUserRole.User == _userId
               && securityUserRole.AssignmentStatus == RoleAssignmentStatus::Enabled
               && (securityUserRole.ValidFrom < DateTimeUtil::utcNow() || securityUserRole.ValidFrom == utcDateTimeNull())
               && (securityUserRole.ValidTo > DateTimeUtil::utcNow() || securityUserRole.ValidTo == utcDateTimeNull());

        return securityPrivilege.RecId != 0;
    }

    public static boolean hasUserSecurityDuty(
        SecurityPrivilegeName _duty
        , UserId _userId = curUserId()
        )
    {
        // To be added
        return false;
    }

    public static boolean hasUserSecurityRole(
    SecurityPrivilegeName _role
    , UserId _userId = curUserId()
        )
    {
        // To be added
        return false;
    }

    public static str userHasNoAccess(UserId _userId = curUserId())
    {
        return strFmt("User \"%1\" has no access to this function. Contact administrators for assistance.", _userId);
    }

}