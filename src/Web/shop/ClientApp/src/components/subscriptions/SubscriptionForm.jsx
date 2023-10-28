import React, { useEffect } from 'react';
import { Form, Input, Select, Checkbox, Button, Modal } from 'antd';
import { NotificationInterval, SubscriptionType } from './SubscriptionsPage';

const { Option } = Select;

export const SubscriptionForm = ({
  open,
  selectedSubscription,
  onCancel = () => {},
  onSubmit = (values) => {}
}) => {

  const [form] = Form.useForm();

  useEffect(() => {
    selectedSubscription?.id && form.setFieldsValue(selectedSubscription);
  }, [form, selectedSubscription])

  return (
    <Modal
      title={`${selectedSubscription ? 'Edit' : 'Add'} Subscription`}
      open={open}
      onCancel={onCancel}
      footer={null}
      forceRender
    >
    <Form layout="vertical" onFinish={onSubmit} form={form}>
      <Form.Item
        name="name"
        label="Name"
        rules={[{ required: true, message: 'Please input your name!' }]}
      >
        <Input />
      </Form.Item>

      <Form.Item
        name="description"
        label="Description"
        rules={[{ required: true, message: 'Please input your description!' }]}
      >
        <Input.TextArea />
      </Form.Item>

      <Form.Item
        name="subscriptionType"
        label="Subscription Type"
        rules={[{ required: true, message: 'Please select your subscription type!' }]}
      >
        <Select placeholder="Select a type">
          <Option value={SubscriptionType.NEW_VEHICLES_FOR_SALE}>New Vehicles For Sale</Option>
          <Option value={SubscriptionType.NEW_VEHICLES_SOLD}>New Vehicles Sold</Option>
        </Select>
      </Form.Item>

      <Form.Item
        name="notificationInterval"
        label="Notification Interval"
        rules={[{ required: true, message: 'Please input your notification interval!' }]}
      >
        <Select placeholder="Select a notification interval">
          <Option value={NotificationInterval.IMMIDIATE}>Immidiate</Option>
          <Option value={NotificationInterval.DAILY}>Daily</Option>
          <Option value={NotificationInterval.WEEKLY}>Weekly</Option>
        </Select>
      </Form.Item>

      <Form.Item
        name="isActive"
        valuePropName="checked"
      >
        <Checkbox>Is Active</Checkbox>
      </Form.Item>

      <Form.Item>
        <div style={{display: 'flex', justifyContent: 'center'}}>
      <Button type="primary" onClick={onCancel} style={{margin: '0px 10px'}}>
          Cancel
        </Button>
        <Button type="primary" htmlType="submit" style={{margin: '0px 10px'}}>
          Submit
        </Button>
        </div>
      </Form.Item>
    </Form>
    </Modal>
  );
};

export default SubscriptionForm;
