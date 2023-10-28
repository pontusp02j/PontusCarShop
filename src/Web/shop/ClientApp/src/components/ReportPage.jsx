import React, { useEffect, useState, useCallback } from 'react';
import { Tabs, Spin, message } from 'antd';
import apiUrl from './api';
import { groupBy } from 'lodash';
import CollapsableTable from './tables/CollapsableTable';
import ErrorPage from '../utilities/ErrorPage';
import PermissionChecker from './PermissionChecker';
import { UserRole } from './NavMenu';

const ReportPageStatus = Object.freeze({
    Idle: 0,
    Loading: 1,
    Error: 2
});

const ActiveTab = Object.freeze({
    Vehicles: '1',
    Users: '2',
});

const ReportPage = () => {
    const parsedUser = JSON.parse(localStorage.getItem('user'));
    const [carData, setCarData] = useState([]);
    const [userData, setUserData] = useState([]);
    const [cars, setCars] = useState([]);
    const [users, setUsers] = useState([]);
    const [pageStatus, setPageStatus] = useState()
    const [activeTab, setActiveTab] = useState(ActiveTab.Vehicles)

    const carColumns = [{ title: 'Vehicles', dataIndex: 'vehicleType', key: 'vehicleType' }, { title: 'Total number of views for vehicle type', dataIndex: 'totalNumberOfViewsForType', key: 'totalNumberOfViewsForType' }];
    const carExpandedColumns = [{ title: 'Model Names', dataIndex: 'modelName', key: 'modelName' }, { title: 'Number of views', dataIndex: 'numberOfViews', key: 'numberOfViews' }];

    const userColumns = [
        { title: 'User Name', dataIndex: 'userName', key: 'userName' },
        { title: 'Email', dataIndex: 'email', key: 'email' },
    ];

    const userExpandedColumns = [
        { title: 'Model Names', dataIndex: 'modelName', key: 'modelName' }
    ];

    const PAGE_STATUS_COMPONENTS = {
        [ReportPageStatus.Loading]: <Spin />,
        [ReportPageStatus.Error]: <ErrorPage />,
        [ReportPageStatus.Idle]: activeTab === ActiveTab.Vehicles ?
            <CollapsableTable
                columns={carColumns}
                data={carData}
                expandedColumns={carExpandedColumns} /> :
            <CollapsableTable
                columns={userColumns}
                data={userData}
                expandedColumns={userExpandedColumns} />,
    };

    const tabItems = [
        {
            key: '1',
            label: 'Vehicles',
            children: PAGE_STATUS_COMPONENTS[pageStatus] || null
        },
        {
            key: '2',
            label: 'Users',
            children: PAGE_STATUS_COMPONENTS[pageStatus] || null
        },
    ];

    useEffect(() => {
        setPageStatus(ReportPageStatus.Loading);
        Promise.all([
            fetch(`${apiUrl}/cars/search?id=-1`)
                .then(response => response.json())
                .then(data => setCars(data)),

            fetch(`${apiUrl}/users`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Could not get users');
                    }
                    return response.json();
                })
                .then(users => setUsers(users))
                .catch(() => {
                    setUsers([]);
                    message.error('Could not get users');
                })
        ])
            .then(() => setPageStatus(ReportPageStatus.Idle))
            .catch(() => setPageStatus(ReportPageStatus.Error));
    }, []);

    const groupCarsByType = useCallback(() => {

        if (!cars) return [];

        return Object.entries(groupBy(cars, 'type')).map(([type, cars]) => ({
            vehicleType: type,
            key: type,
            totalNumberOfViewsForType: cars.reduce((sum, car) => sum + car.numberOfViews, 0),
            cars: cars.map(car => ({
                modelName: car.modelName,
                key: car.id,
                numberOfViews: car.numberOfViews
            })),
        }));

    }, [cars]);

    const getUserData = useCallback(() => {
        if (!users) return [];
        
        return users.map((user, index) => ({
            key: index,
            userName: user.userName,
            email: user.email,
            collapseData: user.viewedCars.map(car => ({
                modelName: car.modelName,
                key: car.id
            })),
        }));
    }, [users]);

    useEffect(() => {
        setUserData(getUserData());
    }, [getUserData]);

    useEffect(() => {
        setCarData(groupCarsByType().map((carGroup, i) => ({
            key: i,
            vehicleType: carGroup.vehicleType,
            totalNumberOfViewsForType: carGroup.totalNumberOfViewsForType,
            collapseData: carGroup.cars.map((car, i) => ({ key: i, modelName: car.modelName, numberOfViews: car.numberOfViews })).sort((a, b) => b.numberOfViews - a.numberOfViews),
        })).sort((a, b) => b.totalNumberOfViewsForType - a.totalNumberOfViewsForType))
    }, [groupCarsByType]);

    return (
        <div>
            <PermissionChecker requiredRole={UserRole.ADMIN} user={parsedUser}></PermissionChecker>
            <Tabs defaultActiveKey={ActiveTab.Vehicles} items={tabItems} onChange={e => setActiveTab(e)} />
        </div>
    );
};

export default ReportPage;