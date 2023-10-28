import React, { useState, useEffect } from 'react';
import apiUrl from './api';
import { CFormSelect } from '@coreui/react';
import { message } from 'antd';

const RegistrationForm = () => {
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [lastname, setLastname] = useState('');
    const [firstname, setFirstname] = useState('');
    const [selectedSubscriptionActivation, setselectedSubscriptionActivation] = useState(true);
    const [availableSubscriptionModels, setAvailableSubscriptionModels] = useState([]);
    const [selectedSubscriptionId, setSelectedSubscriptionId] = useState();
    const [swedishRegions, setSwedishRegions] = useState([]);
    const [selectedRegion, setSelectedRegion] = useState('');
    const [mobilePhoneNumber, setMobilePhoneNumber] = useState('');
    const [isEmailValid, setIsEmailValid] = useState(false);
    const [isUsernameValid, setIsUsernameValid] = useState(false);
    const [isPasswordValid, setIsPasswordValid] = useState(false);
    const [isLastnameValid, setIsLastnameValid] = useState(false);
    const [isFirstnameValid, setIsFirstnameValid] = useState(false);
    const [isMobilePhoneNumberValid, setIsMobilePhoneNumberValid] = useState(false);

    const [validationState, setValidationState] = useState([
        {
            field: 'Email', value: email, isValid: false,
            validationRegex: /^[^\s@]+@[^\s@]+\.[^\s@]+$/
        },
        {
            field: 'Username',
            value: username,
            isValid: false,
            validationCriteria: (value) => /^[a-zA-Z0-9]+$/.test(value) && value.length >= 8 && value.length <= 16
        },
        {
            field: 'Password',
            value: password,
            isValid: false,
            validationCriteria: (value) => /^[a-zA-Z0-9]+$/.test(value) && value.length >= 10 && value.length <= 24
        },
        {
            field: 'Lastname', value: lastname, isValid: false,
            validationCriteria: (value) => value.length >= 2 && value.length <= 56
        },
        {
            field: 'Firstname', value: firstname, isValid: false,
            validationCriteria: (value) => value.length >= 2 && value.length <= 56
        },
        {
            field: 'MobilePhoneNumber', value: mobilePhoneNumber, isValid: false,
            validationRegex: /^[+]?[\d]{8,15}$/
        },
    ]);

    const handleInputChange = (e, field) => {
        const inputValue = e.target.value;

        const updatedValidationState = validationState.map((item) => {
            if (item.field === field) {
                return {
                    ...item,
                    value: inputValue,
                    isValid: validateField(inputValue, item),
                };
            }
            return item;
        });

        const fieldIndex = updatedValidationState.findIndex(item => item.field === field);

        if (fieldIndex !== -1) {
            updatedValidationState[fieldIndex].value = inputValue;
            updatedValidationState[fieldIndex].isValid = validateField(inputValue, updatedValidationState[fieldIndex]);
        }

        const fieldsToUpdate = [
            { name: 'Email', stateSetter: setEmail, validSetter: setIsEmailValid },
            { name: 'Username', stateSetter: setUsername, validSetter: setIsUsernameValid },
            { name: 'Password', stateSetter: setPassword, validSetter: setIsPasswordValid },
            { name: 'Lastname', stateSetter: setLastname, validSetter: setIsLastnameValid },
            { name: 'Firstname', stateSetter: setFirstname, validSetter: setIsFirstnameValid },
            { name: 'MobilePhoneNumber', stateSetter: setMobilePhoneNumber, validSetter: setIsMobilePhoneNumberValid },
        ];

        fieldsToUpdate.forEach((fieldToUpdate) => {
            if (fieldToUpdate.name === field) {
                fieldToUpdate.stateSetter(inputValue);
                fieldToUpdate.validSetter(updatedValidationState[fieldIndex].isValid);
            }
        });

        setValidationState(updatedValidationState);
    }

    useEffect(() => {
        fetch('http://localhost:5286/api/subscriptions')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Could not get subscriptions');
                }
                return response.json();
            })
            .then(subscriptions => {
                setAvailableSubscriptionModels(subscriptions.filter(sub => sub.isActive));
                setSelectedSubscriptionId(subscriptions?.[0]?.id);
            })
            .catch(() => {
                setAvailableSubscriptionModels([]);
            });
    }, []);

    useEffect(() => {
        const fetchSwedishRegions = async () => {
            try {
                const response = await fetch(`${apiUrl}/swedishregions`);
                const data = await response.json();
                const values = data.variables.pop().valueTexts;
                setSwedishRegions(values);
            } catch (error) {
                console.error('Error fetching Swedish regions:', error);
            }
        }

        fetchSwedishRegions();
    }, [])


    const validateField = (value, field) => {
        if (field.validationRegex) {
            return field.validationRegex.test(value);
        } else if (field.validationCriteria) {
            return field.validationCriteria(value);
        }
        return true;
    }

    const handleChange = (event, field) => {
        const selectedValue = event.target.value;

        if (field === "SubscriptionActivation") {
            setselectedSubscriptionActivation(selectedValue);
        } else if (field === "SubscriptionModel") {
            setSelectedSubscriptionId(selectedValue)
        } else {
            setSelectedRegion(selectedValue);
        }
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!isEmailValid || !isFirstnameValid || !isLastnameValid
            || !isPasswordValid || !isUsernameValid || !isMobilePhoneNumberValid) {
            resetFields();
            return;
        }

        const formData = {
            Email: email,
            Username: username,
            Password: password,
            Lastname: lastname,
            Firstname: firstname,
            SwedishRegion: selectedRegion,
            MobilePhoneNumber: mobilePhoneNumber,
            SubscribeToNewCars: selectedSubscriptionActivation,
            SubscriptionId: selectedSubscriptionId,
        }

        try {
            const response = await fetch(`${apiUrl}/users/create`, {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            (response.ok) ? message.success('User Registered')
                : message.success('User could not be registered');
        } catch (error) {
            console.error(error);
        }
    }

    const resetFields = () => {
        setEmail('');
        setUsername('');
        setPassword('');
        setLastname('');
        setFirstname('');
        setMobilePhoneNumber('');
        setSelectedRegion('');
        setIsEmailValid(false);
        setIsUsernameValid(false);
        setIsPasswordValid(false);
        setIsLastnameValid(false);
        setIsFirstnameValid(false);
        setIsMobilePhoneNumberValid(false);
    };

    return (
        <form onSubmit={handleSubmit}>
            {validationState.map(item => (
                <div key={item.field} className="form-group">
                    {!item.isValid &&
                        <div className="p-3 mb-2 bg-danger text-white mt-4 mb-4">{`${item.field} is not valid.`}</div>
                    }

                    <label htmlFor={item.field}>{item.field}:</label>
                    <input
                        type={item.field === 'Password' ? 'password' : 'text'}
                        id={item.field.toLowerCase()}
                        value={
                            item.field === 'Email'
                                ? email
                                : item.field === 'Username'
                                    ? username
                                    : item.field === 'Password'
                                        ? password
                                        : item.field === 'Lastname'
                                            ? lastname
                                            : item.field === 'Firstname'
                                                ? firstname
                                                : item.field === 'MobilePhoneNumber'
                                                    ? mobilePhoneNumber
                                                    : ''
                        }
                        onChange={(e) => handleInputChange(e, item.field)}
                        placeholder={item.field}
                        className="form-control"
                    />
                </div>
            ))}

            <div className='mt-3'>
                <label htmlFor="swedishRegion">Swedish Region:</label>
                <CFormSelect
                    className='w-25 mb-3'
                    onChange={(e) => handleChange(e, 'selectedRegion')}
                    value={selectedRegion}
                >
                    {swedishRegions.map((region, i) => (
                        <option key={region} value={region}>
                            {region}
                        </option>
                    ))}
                </CFormSelect>
            </div>

            <div className='mt-3'>
                <label htmlFor="SubscriptionActivation">Subscription Activation:</label>
                <CFormSelect
                    className='w-25 mb-3'
                    onChange={(e) => handleChange(e, "SubscriptionActivation")}
                    value={selectedSubscriptionActivation}
                >
                    <option key={false} value={false}>False</option>
                    <option key={true} value={true}>True</option>
                </CFormSelect>
            </div>

            <div className='mt-3'>
                <label htmlFor="SubscriptionModel">Subscription Model:</label>
                <CFormSelect
                    className='w-25 mb-3'
                    onChange={(e) => handleChange(e, "SubscriptionModel")}
                    value={selectedSubscriptionId}
                >
                    {availableSubscriptionModels.map((model, i) => {
                        return <option key={i} value={model.id}>{model.name}</option>
                    })}
                </CFormSelect>
            </div>

            <button type="submit" className="btn btn-primary mb-4">Register</button>
        </form>
    );
};

export default RegistrationForm;
