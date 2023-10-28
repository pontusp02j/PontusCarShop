import React, { useState } from 'react';
import apiUrl from './api';

const LoginForm = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleUsernameChange = (event) => {
    setUsername(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (username.length < 8 || username.length > 16
      || password.length < 10 || password.length > 24) {
      setError('Username should be 8 to 16 character long'
        + 'and password should be 10 to 24 characters long.');
        return;
    }

    try {
      const response = await fetch(`${apiUrl}/login/user`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password }),
      });

      if (response.ok) {
        const user = await response.json();
        if (user != null) {
          localStorage.setItem('user', JSON.stringify(user));
          setIsLoggedIn(true);
          window.location.href = `/`;
        } else {
          setError('Invalid username or password.');
        }
      } else {
        setError('An error occurred during login.');
      }

    } catch (error) {
      console.log(error)
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {error &&
        <div className="p-3 mb-2 bg-danger text-white mt-4 mb-4">{`${error} is not valid.`}</div>
      }
      <div className='form-group mt-2 mb-2'>
        <label htmlFor="username">Username:</label>
        <input type="text" id="username" className='form-control' value={username} onChange={handleUsernameChange} />
      </div>
      <div className='form-group mt-2 mb-2'>
        <label htmlFor="password">Password:</label>
        <input type="password" id="password" className='form-control' value={password} onChange={handlePasswordChange} />
      </div>
      <button type="submit" className='mb-4 mt-4'>Login</button>
    </form>
  );
};

export default LoginForm;
