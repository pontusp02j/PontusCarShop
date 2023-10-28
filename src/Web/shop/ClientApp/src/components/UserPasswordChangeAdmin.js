import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import apiUrl from './api';
import { CFormSelect } from '@coreui/react';
import PermissionChecker from './PermissionChecker';
import { UserRole } from './NavMenu';

const UserPasswordChangeAdmin = () => {
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [validationMessage, setValidationMessage] = useState('');
    const [user] = useState(localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')) : null);
    const [users, setUsers] = useState([]);
    const [selectedUser, setUserId] = useState(1);

    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
    }

    useEffect(() => {
            fetchUsers();
    }, []);

    const handleChange = (event, field) => {
        const selectedValue = event.target.value;
        setUserId(selectedValue);
    }

    const fetchUsers = async () => {
        const fetchUsersUrl = `${apiUrl}/users`;
        try {
            const response = await fetch(fetchUsersUrl);
            const usersResponse = await response.json();
            setUsers(usersResponse);
        } catch (error) {
            console.error(error);
        }
    }


    const handleSubmit = async (event) => {
        event.preventDefault();

        try {
            const response = await fetch(`${apiUrl}/users/updatePassword`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ id: parseInt(selectedUser), password }),
            });

            if (response.ok) {
                const user = await response.json();
                if (user) {
                    setValidationMessage(`Change password for user: ${user.email}`);
                } else {
                    setError(`An error occurred while processing your request.
                        please try again later. If the issue persists, please contact
                        your IT administrator, couln't fetch the user back to the client`);
                }
            } else {
                setError(`An error occurred happend when changing password. 
                    Error Code: ${response.status}`);
            }

        } catch (error) {
            setError(`An error occurred happend when changing password. 
                Error Code: ${error.code}`);
        }
    }

    return (
        <form onSubmit={handleSubmit}>
            <PermissionChecker requiredRole={UserRole.ADMIN} user={user}></PermissionChecker>
            {(error || validationMessage) && (
                <div className="p-3 mb-2 bg-warning text-white mt-4 mb-4">
                    {error && `${error} is not valid.`}
                    {validationMessage && `${validationMessage}.`}
                </div>
            )}

            {error &&
                <div className="p-3 mb-2 bg-danger text-white mt-4 mb-4">{`${error} is not valid.`}</div>
            }

            <div className='mt-3'>
                <label htmlFor="users">Users Dropdown:</label>
                <CFormSelect
                    className='w-25 mb-3'
                    onChange={(e) => handleChange(e, 'user')}
                    value={selectedUser}
                >
                    {users.length > 0 && users.map((user, i) => (
                        <option key={user.id} value={user.id}>
                            {user.email}
                        </option>
                    ))}
                </CFormSelect>
            </div>

            <div className='form-group mt-2 mb-2'>
                <label htmlFor="password">Password:</label>
                <input type="password" id="password" className='form-control' value={password} onChange={handlePasswordChange} />
            </div>
            <button type="submit" className='mb-4 mt-4 btn btn-primary'>Change password</button>
        </form>
    );
};

export default UserPasswordChangeAdmin;
