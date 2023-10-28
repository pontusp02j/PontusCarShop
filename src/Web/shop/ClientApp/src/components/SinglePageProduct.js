import React, { Component } from 'react';
import apiUrl from './api';
import { Link } from 'react-router-dom';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';

export class SinglePageProduct extends Component {
  static displayName = SinglePageProduct.name;

  constructor(props) {
    super(props);
    this.state = {
      cars: [],
      preview: false,
      imageSrc: "",
    };

    this.togglePreview = this.togglePreview.bind(this);
  }

  componentDidMount() {
    this.fetchCar();
  }

  togglePreview() {
    this.setState(prevState => ({
      preview: !prevState.preview
    }));
  }

  handleFileChanged = (base64String) => {
    const contentType = 'image/png';
    const byteCharacters = atob(base64String);
    const byteArrays = [];
  
    for (let offset = 0; offset < byteCharacters.length; offset += 512) {
      const slice = byteCharacters.slice(offset, offset + 512);
      const byteNumbers = new Array(slice.length);
  
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
  
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
  
    const blob = new Blob(byteArrays, { type: contentType });
  
    const reader = new FileReader();
    reader.onload = (evt) => {
      const binaryData = evt.target.result;
  
      const arrayBuffer = new ArrayBuffer(binaryData.length);
      const uint8Array = new Uint8Array(arrayBuffer);
      for (let i = 0; i < binaryData.length; i++) {
        uint8Array[i] = binaryData.charCodeAt(i);
      }
  
      const file2 = new File([arrayBuffer], "image.png", { type: "image/png" });
  
      const imageUrl = URL.createObjectURL(file2);
      this.setState({ imageSrc: imageUrl });
    };
  
    reader.readAsBinaryString(blob);
  }
  

  async fetchCar() {
    try {
      const segments = window.location.pathname.split('/');
      const lastSegment = segments[segments.length - 1];
      const response = await fetch(`${apiUrl}/cars/search?id=${lastSegment}`);
      const listOfCars = await response.json();
      this.handleFileChanged(listOfCars[0].image);
      this.setState({ cars: listOfCars });
    }
    catch (error) {
      console.error(error);
    }
  }

  render() {
    const carList = this.state.cars.map(car => (
      <div className="container" key={car.id}>
        <div className="row">
          <summary className="col">
            <img src={this.state.imageSrc} alt={car.modelName} 
            className="img-thumbnail img-fluid rounded w-25 cursor-pointer" 
            data-toggle="preview" data-target="#mypreview2" onClick={this.togglePreview} />
          </summary>
          <Modal isOpen={this.state.preview} toggle={this.togglePreview} className={this.props.className}>
            <ModalHeader toggle={this.togglePreview}>Image Preview</ModalHeader>
            <ModalBody>
              <img src={this.state.imageSrc} alt={car.image} className="img-fluid" />
            </ModalBody>
          </Modal>
          <label>Description: {car.description}</label>
          <label>Engine size: {car.engineSize}</label>
          <label>Last serving date: {car.lastServingDate}</label>
          <label>Next serving date: {car.nextServingDate}</label>
          <label>Model year: {car.modelYear}</label>
          <label>Model name: {car.modelName}</label>
          <label>Car brand: {car.brand}</label>
          <label>Mileage: {car.mileage}</label>
          <label>Doors: {car.doors}</label>
          <label>Type: {car.type}</label>
          {(car.status === 1 && car.status > 0) ? (
          <label>Sold</label>
          ) : <label>Sale</label>}
          <Link className="btn btn-primary m-3 w-100 p-2" to={`/products/create?id=${car.id}`}>
            Edit
          </Link>
        </div>
      </div>
    ));

    return (
      <div>
        {carList}
      </div>
    );
  }
}

export default SinglePageProduct;