import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

import apiUrl from '../api';
import { Form, message, Input, Checkbox, Select, Button } from 'antd';

const MyPage = () => {

  const { id } = useParams();
  const [swedishRegions, setSwedishRegions] = useState([]);
  const [user, setUser] = useState({
    userName: '',
    email: '',
    emailVerified: false,
    firstName: '',
    lastName: '',
    swedishRegion: '',
    mobilePhoneNumber: '',
    createdUtc: '',
    subscribeToNewCars: false,
    lastNotified: '',
    lastSubscribed: '',
});
const [availableSubscriptionModels, setAvailableSubscriptionModels] = useState([]);
const [form] = Form.useForm();
  
useEffect(() => {
  const fetchData = async () => {
    try {
      const [userRes, subscriptionsRes, regionsRes] = await Promise.all([fetch(`${apiUrl}/users/${id}`), fetch(`${apiUrl}/subscriptions`), fetch(`${apiUrl}/swedishregions`)]);

      if(!userRes.ok || !subscriptionsRes.ok || !regionsRes.ok){
        throw new Error();
      }
      var user = await userRes.json()
      var subscriptions = await subscriptionsRes.json()
      var regionsData = await regionsRes.json();
      setSwedishRegions(regionsData.variables.pop().valueTexts);
      setUser(user);
      setAvailableSubscriptionModels(subscriptions?.filter(sub => sub.isActive));
    } catch (error) {
      message.error('Something went wrong');
    }
  };

  fetchData();
}, [id]);

useEffect(() => {
  user?.id && form.setFieldsValue(user);
}, [form, user])

const handleSubmit = async (formData) => {
  try {

    const response = await fetch(`${apiUrl}/users/${user.id}`, {
      method: 'PUT',
      body: JSON.stringify(formData),
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (response.ok) {

      const updatedUser = await response.json();
      const cachedUser = JSON.parse(localStorage.getItem('user'));

      if(cachedUser.username !== updatedUser?.userName){
        cachedUser.username = updatedUser.userName
        localStorage.setItem('user', JSON.stringify(cachedUser));
      }
      window.location.reload();

    } else {
      throw new Error(`Request failed with status ${response.status}`);
    }
  } catch (error) {
    message.error('Could not update user details');
  }
};

return (
<div style={{ width: "50%", margin: "0 auto" }}>
  <h2 style={{ textAlign: "center", marginBottom: "1rem" }}>My Page</h2>
  <Form  form={form} layout="vertical" onFinish={handleSubmit}>
    <Form.Item
      name="userName"
      label="Username"
      rules={[{ required: true, message: 'Please input your username!' }]}
    >
      <Input value={user.userName} />
    </Form.Item>

    <Form.Item
      name="email"
      label="Email"
      rules={[{ required: true, message: 'Please input your email!' }]}
    >
      <Input value={user.email} />
    </Form.Item>

    <Form.Item
      name="firstName"
      label="First Name"
      rules={[{ required: true, message: 'Please input your first name!' }]}
    >
      <Input value={user.firstName} />
    </Form.Item>

    <Form.Item
      name="lastName"
      label="Last Name"
      rules={[{ required: true, message: 'Please input your last name!' }]}
    >
      <Input value={user.lastName} />
    </Form.Item>

    <Form.Item
      name="swedishRegion"
      label="Region"
      rules={[{ required: true, message: 'Please input your region!' }]}
    >
      <Select 
        placeholder="Select a region" 
      >
        {swedishRegions.map((model, i) => {
          return <Select.Option key={i} value={model}>{model}</Select.Option>
        })}
      </Select>
    </Form.Item>

    <Form.Item
      name="mobilePhoneNumber"
      label="Mobile Phone"
      rules={[{ required: true, message: 'Please input your mobile phone number!' }]}
    >
      <Input value={user.mobilePhoneNumber} />
    </Form.Item>

    <Form.Item
      name="subscribeToNewCars"
      valuePropName="checked"
    >
      <Checkbox checked={user.subscribeToNewCars}>Subscribe to New Cars</Checkbox>
    </Form.Item>

    <Form.Item
      name="subscriptionId"
      label="Subscription"
      rules={[{ required: true, message: 'Please select your subscription!' }]}
    >
      <Select 
        placeholder="Select a Subscription" 
      >
        {availableSubscriptionModels.map((model, i) => {
          return <Select.Option key={i} value={model.id}>{model.name}</Select.Option>
        })}
      </Select>
    </Form.Item>

    <Form.Item>
      <div style={{display: 'flex', justifyContent: 'center'}}>
        <Button type="primary" htmlType="submit" style={{margin: '0px 10px'}}>Save</Button>
      </div>
    </Form.Item>
  </Form>
</div>
)};

export default MyPage;
