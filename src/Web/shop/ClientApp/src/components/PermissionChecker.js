import React, { useEffect } from 'react';

const PermissionChecker = ({ requiredRole, user }) => {
    useEffect(() => {
        if (user != null) {
            if (user.permissionLevel === requiredRole) {
                return;
            } else {
                window.location.href = `/`;
            }
        } else {
            window.location.href = `/`;
        }
    }, [requiredRole, user]);

    return null;
}

export default PermissionChecker;
