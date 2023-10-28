import React, { Component } from 'react';
import apiUrl from './api';
import { CFormSelect } from '@coreui/react';
import PermissionChecker from './PermissionChecker';
import { UserRole } from './NavMenu';

export class CreateCarProduct extends Component {
  static displayName = CreateCarProduct.name;

  constructor(props) {
    super(props);
    const user = JSON.parse(localStorage.getItem('user'));
    this.state = {
      id: 0,
      status: '',
      modelName: '',
      description: '',
      mileage: '',
      brand: '',
      engineSize: 0,
      modelYear: new Date().toISOString().slice(0, 10),
      lastServingDate: new Date().toISOString().slice(0, 10),
      nextServingDate: new Date().toISOString().slice(0, 10),
      image: '',
      imageSrc: '',
      type: '',
      doors: '',
      numberOfViews: 0,
      user: user ? user : null 
    }

    this.handelCreateCar = this.handelCreateCar.bind(this);
    this.handleEditCar = this.handleEditCar.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  handleFileChanged = (event) => {
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = (evt) => {
      const fileContent = evt.target.result;
      const base64String = fileContent.split(',')[1];

      const binaryData = atob(base64String);

      const arrayBuffer = new ArrayBuffer(binaryData.length);
      const uint8Array = new Uint8Array(arrayBuffer);
      for (let i = 0; i < binaryData.length; i++) {
        uint8Array[i] = binaryData.charCodeAt(i);
      }

      const file2 = new File([arrayBuffer], "image.png", { type: "image/png" });

      const imageUrl = URL.createObjectURL(file2);
      this.setState({ imageSrc: imageUrl, image: base64String });
    };
  };

  handleEditCar = async (e) => {
    e.preventDefault();
    const data = {
      id: this.state.id,
      modelName: this.state.modelName,
      description: this.state.description,
      mileage: parseFloat(this.state.mileage),
      brand: this.state.brand,
      engineSize: parseInt(this.state.engineSize),
      numberOfViews: this.state.numberOfViews,
      modelYear: this.state.modelYear,
      nextServingDate: this.state.nextServingDate,
      lastServingDate: this.state.lastServingDate,
      type: this.state.type,
      status: parseInt(this.state.status),
      doors: parseInt(this.state.doors),
      image: this.state.image,
    };

    try {
      const response = await fetch(`${apiUrl}/cars/update`, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
        },
      });

      const fetchedData = await response.json();

      if (response.ok) {
        window.location.href = `/products/${fetchedData.id}`;
      } else {
        throw new Error(`Request failed with status ${response.status}`);
      }
    } catch (error) {
      console.error(error);
    }
  }

  handelCreateCar = async (e) => {
    e.preventDefault();
    const data = {
      id: this.state.id,
      modelName: this.state.modelName,
      description: this.state.description,
      mileage: parseFloat(this.state.mileage),
      brand: this.state.brand,
      engineSize: parseInt(this.state.engineSize),
      modelYear: this.state.modelYear,
      nextServingDate: this.state.nextServingDate,
      lastServingDate: this.state.lastServingDate,
      type: this.state.type,
      status: parseInt(this.state.status),
      doors: parseInt(this.state.doors),
      image: this.state.image,
    };

    try {
      const response = await fetch(`${apiUrl}/cars/create`, {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
        },
      });

      const fetchedData = await response.json();
      (response.ok) ? window.location.href = `/products/${fetchedData.id}`
        : console.log(`Error: ${response}`);
    } catch (error) {
      console.error(error);
    }
  }

  componentDidMount() {
    const searchParams = new URLSearchParams(window.location.search);
    const fetchedId = searchParams.get("id");
    fetchedId && this.setState({ id: fetchedId });
    if (fetchedId) {
      this.fetchCars(fetchedId);
    }
  }

  fetchCars = async (id) => {
    try {
      const response = await fetch(`${apiUrl}/cars/search?id=${id}`);
      const singleCarResponse = await response.json();
  
      this.setState({
        ...singleCarResponse.pop()
      });
    } catch (error) {
      console.error(error);
    }
  };
  
  
  handleChange(event) {
    const selectedValue = event.target.value;
    if (selectedValue !== this.state.status && event.target.name === 'status') {
      this.setState({ status: selectedValue });
    }
  }

  handleFileChange = (event) => {
    this.setState({ file: event.target.files[0] });
  }


  render() {
    return (
      <form onSubmit={this.handelCreateCar}>
        <PermissionChecker requiredRole={UserRole.ADMIN} user={this.state.user}></PermissionChecker>

        <div className="form-row">
          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer01">Model Name</label>
            <input type="text" className="form-control" id="validationServer01" value={this.state.modelName} onChange={event => this.setState({ modelName: event.target.value })} placeholder="Model name" />
          </div>
          <div className="col-md-4 mb-3">
            <div className="form-group">
              <label htmlFor="exampleFormControlTextarea1">Description</label>
              <textarea className="form-control" id="exampleFormControlTextarea1" value={this.state.description} onChange={event => this.setState({ description: event.target.value })} placeholder='description' rows="3"></textarea>
            </div>
          </div>

          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer02">Mileage</label>
            <input type="number" className="form-control" id="validationServer01" value={this.state.mileage} onChange={event => this.setState({ mileage: event.target.value })} placeholder="Milage" />
          </div>

          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer01">Brand</label>
            <input type="text" className="form-control" id="validationServer01" value={this.state.brand} onChange={event => this.setState({ brand: event.target.value })} placeholder="Brand" />
          </div>

          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer01">Type</label>
            <input type="text" className="form-control" id="validationServer01" value={this.state.type} onChange={event => this.setState({ type: event.target.value })} placeholder="Type" />
          </div>

          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer02">Engine size</label>
            <input type="number" className="form-control" id="validationServer01" value={this.state.engineSize} onChange={event => this.setState({ engineSize: event.target.value })} placeholder="Engine size" />
          </div>

          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer02">Doors</label>
            <input type="number" className="form-control" id="validationServer01" value={this.state.doors} onChange={event => this.setState({ doors: event.target.value })} placeholder="Doors" />
          </div>

          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer02">Model Year</label>
            <input type="date" className="form-control" id="validationServer01" value={this.state.modelYear} onChange={event => this.setState({ modelYear: event.target.value })} />
          </div>

          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer02">Last Serving Date</label>
            <input type="date" className="form-control" id="validationServer01" value={this.state.lastServingDate} onChange={event => this.setState({ lastServingDate: event.target.value })} />
          </div>


          <div className="col-md-4 mb-3">
            <label htmlFor="validationServer02">Next Serving Date</label>
            <input type="date" className="form-control" id="validationServer01" value={this.state.nextServingDate} onChange={event => this.setState({ nextServingDate: event.target.value })} />
          </div>

          <CFormSelect
            className='w-25 mb-3'
            name="status"
            value={this.state.status}
            onChange={this.handleChange}
          >
            <option value="0">Select a status</option>
            <option value="1">Sold</option>
            <option value="2">Sale</option>
          </CFormSelect>

        </div>

        <div className="custom-file">
          <input
            type="file"
            className="custom-file-input"
            id="validatedCustomFile"
            onChange={this.handleFileChanged}
          /><br />
          {this.state.imageSrc && <img src={this.state.imageSrc} alt="Uploaded Car" className="w-25 mt-3" />}
        </div>
        <button onClick={this.state.id ? this.handleEditCar : this.handelCreateCar} type="submit" className="btn btn-primary mt-3 mb-4">{`${this.state.id ? 'Edit' : 'Add'} Car`}</button>
      </form>
    );
  }
}

export default CreateCarProduct;