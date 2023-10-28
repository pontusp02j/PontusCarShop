import React, { useState, useEffect } from 'react';
import { Select, Table } from 'antd';
import moment from 'moment';
import apiUrl from '../api';

const UserFilterOptions = Object.freeze({
  ALL_USERS : "All Users",
  SUBSCRIBERS: "Subscribers",
  NON_SUBSCRIBERS: "Non Subscribers",
})


export const UsersPage = () => {
  const [users, setUsers] = useState([]);
  const [visibleUsers, setVisibleUsers] = useState([]);
  const [selectedFilterOption, setSelectedFilterOption] = useState(UserFilterOptions.ALL_USERS);

  useEffect(() => {
    fetch(`${apiUrl}/users`)
    .then(response => {
      if (!response.ok) {
        throw new Error('Could not get users');
      }
      return response.json();
    })
    .then(users => {
      setUsers(users);
    })
    .catch(() => {
      setUsers([]);
    });
  }, []);

  const filterOptions = {
    [UserFilterOptions.SUBSCRIBERS]: user => user.subscribeToNewCars,
    [UserFilterOptions.NON_SUBSCRIBERS]: user => !user.subscribeToNewCars,
    default: () => true
  };
  
  useEffect(() => {
    const filterFunction = filterOptions[selectedFilterOption] || filterOptions.default;
    setVisibleUsers(users.filter(filterFunction));
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedFilterOption, users]);

  const columns = [
    {
      title: 'Full Name',
      dataIndex: 'fullName',
      key: 'fullName',
      align: 'left',
    },
    {
      title: 'Last Subscribed',
      dataIndex: 'lastSubscribed',
      key: 'lastSubscribed',
      align: 'center',
      render: date => moment(date).format('YYYY-MM-DD'),
      sorter: (a, b) => moment(a.createdUtc).unix() - moment(b.createdUtc).unix(),
    },
    {
      title: 'Subscribing',
      dataIndex: 'subscribeToNewCars',
      key: 'subscribeToNewCars',
      align: 'center',
      render: item => item?.toString(),
    },
  ];

  return (
    <div>
      <h1 style={{margin: '50px 0px'}}>Users</h1>
      <Select
      defaultValue={UserFilterOptions.ALL_USERS}
      style={{
        width: 200,
        marginBottom: 20
      }}
      onChange={option => setSelectedFilterOption(option)}
      options={Object.values(UserFilterOptions).map(val => ({
        label: val,
        value: val
      }))}
    />
      <Table columns={columns} dataSource={visibleUsers} rowKey="id"/>
    </div>
  )
};    
