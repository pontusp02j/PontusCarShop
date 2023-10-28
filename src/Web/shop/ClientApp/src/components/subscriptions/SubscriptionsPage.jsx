import React, { useState, useEffect } from 'react';
import { Table, Space, Button, Dropdown } from 'antd';
import { DownOutlined, PlusOutlined } from '@ant-design/icons';
import { SubscriptionForm } from './SubscriptionForm';
import moment from 'moment';
import apiUrl from '../api';

const Overlay = Object.freeze({
  ADD: 'Add',
  EDIT: 'Edit',
  NONE: 'None'
});

export const NotificationInterval = Object.freeze({
  IMMIDIATE: 0,
  DAILY: 1,
  WEEKLY: 2
});

export const SubscriptionType = Object.freeze({
  NEW_VEHICLES_FOR_SALE: 0,
  NEW_VEHICLES_SOLD: 1,
});

export const SubscriptionsPage = () => {
  const [subscriptions, setSubscriptions] = useState([]);
  const [selectedSubscription, setSelectedSubscription] = useState();
  const [overlay, setOverlay] = useState(Overlay.NONE);

  const getPropertyNameByValue = (object, value) => {
    return Object.keys(object).find(key => object[key] === value);
  }

  useEffect(() => {
    fetch(`${apiUrl}/subscriptions`)
    .then(response => {
      if (!response.ok) {
        throw new Error('Could not get subscriptions');
      }
      return response.json();
    })
    .then(subscriptions => {
      setSubscriptions(subscriptions);
    })
    .catch(() => {
      setSubscriptions([]);
    });
  }, []);

  const handleSubmit = async (subscriptionData) => {
    let method = 'POST';
    let routeEnd = 'create'

    if(selectedSubscription){
      method = 'PUT';
      routeEnd = 'update';
      subscriptionData = {...subscriptionData, id : selectedSubscription.id}
    }

    const responseAction = (subscription) => selectedSubscription ? setSubscriptions(prevItems => prevItems?.map(item => item.id === subscription.id ? subscription : item)) : setSubscriptions([...subscriptions, subscription])

    try {
      const response = await fetch(`http://localhost:5286/api/subscriptions/${routeEnd}`, {
        method: method,
        body: JSON.stringify(subscriptionData),
        headers: {
          'Content-Type': 'application/json',
        },
      });

      const subscriptionResponse = await response.json();
      (response.ok) ? responseAction(subscriptionResponse)
        : console.log(`Error: ${response}`);
    } catch (error) {
      console.error(error);
    }finally{
      selectedSubscription && setSelectedSubscription(undefined);
      setOverlay(Overlay.NONE);
    }
  }

  const items = [
    {
      label: 'Edit',
      key: 'edit',
      onClick: () => setOverlay(Overlay.EDIT)
    },
  ];

  const columns = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      align: 'center',
    },
    {
      title: 'Description',
      dataIndex: 'description',
      key: 'description',
      align: 'center',
    },
    {
      title: 'Creation Date',
      dataIndex: 'CreationDate',
      key: 'creationDate',
      align: 'center',
      render: date => moment(date).format('YYYY-MM-DD'),
    },
    {
      title: 'Subscription Type',
      dataIndex: 'subscriptionType',
      key: 'subscriptionType',
      align: 'center',
      render: item => getPropertyNameByValue(SubscriptionType, item)
    },
    {
      title: 'Notification Interval',
      dataIndex: 'notificationInterval',
      key: 'notificationInterval',
      align: 'center',
      render: item => getPropertyNameByValue(NotificationInterval, item),
    },
    {
      title: 'Active',
      dataIndex: 'isActive',
      key: 'isActive',
      align: 'center',
      render: item => item?.toString(),
    },
    {
      title: 'Actions',
      key: 'actions',
      align: 'center',
      render: (record) => {
        return (
        <Dropdown menu={{ items }} trigger={['click']} onClick={() => setSelectedSubscription(record)}>
          <Space style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
            <DownOutlined style={{ cursor: 'pointer' }} />
          </Space>
        </Dropdown>
      )},
    },
  ];

  return (
    <div>
      <h1 style={{margin: '50px 0px'}}>Subscriptions</h1>
      <Button 
        type="primary" 
        icon={<PlusOutlined />} 
        style={{ marginBottom: '20px' }}
        onClick={() => setOverlay(Overlay.ADD)}
      >
        Add Subscription
      </Button>
      <Table columns={columns} dataSource={subscriptions} rowKey="id"/>
      <SubscriptionForm
        open={overlay === Overlay.ADD || overlay === Overlay.EDIT}
        selectedSubscription={selectedSubscription}
        onCancel={() => setOverlay(Overlay.NONE)}
        onSubmit={handleSubmit}
      />
    </div>
  );
};

export default SubscriptionsPage;
