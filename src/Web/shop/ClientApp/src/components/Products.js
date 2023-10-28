import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import CarFilters from './CarFilters';
import apiUrl from './api';
import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import { replaceItemById } from '../utilities/utilityFunctions';

export class Products extends Component {
  static displayName = Products.name;

  constructor(props) {
    super(props);
    this.state = {
      cars: [],
      filterType: null,
      searchValue: null,
      statusValue: null,
      togglePreview: false,
      selectedImage: null,
    };

    this.types = [];
    this.statusTypes = [];
    this.handleFilterChange = this.handleFilterChange.bind(this);
    this.handleSearchValue = this.handleSearchValue.bind(this);
    this.handleStatusValue = this.handleStatusValue.bind(this);
    this.updateCarWithUserIdAndNumberOfViews = this.updateCarWithUserIdAndNumberOfViews.bind(this);
    this.togglePreview = this.togglePreview.bind(this);
  }

  componentDidMount() {
    this.fetchCars();
  }

  componentDidUpdate(prevProps, prevState) {
    if (
      this.state.filterType !== prevState.filterType ||
      this.state.searchValue !== prevState.searchValue ||
      this.state.statusValue !== prevState.statusValue
    ) {
      this.fetchCars();
    }
  }

  handleImageClick = (event, image) => {
    event.preventDefault();
    this.setState({
      togglePreview: true,
      selectedImage: image,
    });
  };

  updateCarWithUserIdAndNumberOfViews = async (carId) => {
    let car = this.state.cars?.find(car => car.id === carId);
    const user = JSON.parse(localStorage.getItem('user'));

    if(!car?.id){
      return;
    }

    if(user?.id){
      try {
        const response = await fetch(`${apiUrl}/cars/createCarUserRelation`, {
          method: 'POST',
          body: JSON.stringify({usersId: user.id, viewedCarsId: car.id}),
          headers: {
            'Content-Type': 'application/json',
          },
        });

        if (!response.ok) {
          throw new Error(`Request failed with status ${response.status}`);
        }
        
      } catch (error) {
        console.error(error);
      }
    }

    //TODO - Update number of views of car here when we have db support for it
    car = {...car, numberOfViews: ++car.numberOfViews}
    try {
      const response = await fetch(`${apiUrl}/cars/update`, {
        method: 'PUT',
        body: JSON.stringify(car),
        headers: {
          'Content-Type': 'application/json',
        },
      });

      const updatedCar = await response.json();

      if (response.ok) {
        this.setState({
          cars: replaceItemById(this.state.cars, updatedCar?.id, updatedCar)
        });
      } else {
        throw new Error(`Request failed with status ${response.status}`);
      }
    } catch (error) {
      console.error(error);
    }

  }

  filterOptionUrl(filterOptions) {
    const allFiltersEmpty = Object.values(filterOptions).every(
      value => value === null || value === '' || value === false
    );

    const queryParameters = Object.entries(filterOptions)
      .filter(([key, value]) => value !== null && value !== '' && value !== false)
      .map(([key, value]) => {
        if (key === 'description' && (this.state.filterType === null || this.state.filterType === '')) {
          const formattedValue = encodeURIComponent(value.trim());
          return `${key}=${formattedValue}&type=${formattedValue}`;
        } else {
          const formattedValue = encodeURIComponent(value);
          return `${key}=${formattedValue}`;
        }
      })
      .join('&');
    return `${apiUrl}/cars/search${allFiltersEmpty ? '?id=-1' : ''}${queryParameters ? `?${queryParameters}` : ''}`;
  }

  fetchCars() {
    let url = this.filterOptionUrl(
      {
        type: this.state.filterType,
        description: this.state.searchValue,
        status: this.state.statusValue
      }
    );

    fetch(url)
      .then(response => response.json())
      .then(listOfCars => {

        this.setState({
          cars: listOfCars
        });

        if (url.includes("id=-1")) {
          this.types = Array.from(new Set(listOfCars.map(car => car.type)));
          this.statusTypes = Array.from(new Set(listOfCars.map(car => car.status)));
        }
      })
      .catch(error => console.error(error));
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

  handleFilterChange(value) {
    this.setState({ filterType: value === "" ? null : value });
  }

  handleSearchValue(value) {
    this.setState({ searchValue: value === null ? null : value });
  }

  handleStatusValue(value) {
    this.setState({ statusValue: value === null ? null : value });
  }

  togglePreview = () => {
    this.setState(prevState => ({
      togglePreview: !prevState.togglePreview
    }));
  }

  render() {
    const carList = this.state.cars
    .sort((a, b) => b.id - a.id)
    .map(car => (
      <div className="col-md-4 mt-2" key={car.id}>
        <summary className="col">
          <img src={`data:image/png;base64,${car.image}`} alt={car.name}
            style={{maxBlockSize: '15em'}}
            className="img-thumbnail img-fluid rounded cursor-pointer w-100"
            onClick={(event) => this.handleImageClick(event, car.image)}
          />
        </summary>

        <p className="text-center">{car.modelName}</p>
        <Link className="btn btn-primary w-100 mb-4" to={`/products/${car.id}`} onClick={() => this.updateCarWithUserIdAndNumberOfViews(car.id)}>
          View Details of The Car
        </Link>
      </div>
    ));

    return (
      <div className="row">
        <h4>Filters</h4>
        <CarFilters
          filterType={this.types}
          currentFilter={this.state.filterType}
          onFilterChange={this.handleFilterChange}
          onSearchChange={this.handleSearchValue}
          filterStatusType={this.statusTypes}
          currentStatusFilter={this.state.statusValue}
          onStatusChange={this.handleStatusValue}
        />

        <Link className="btn btn-primary m-3 w-100 p-2" to={`/products/create`}>
          Add +
        </Link>

        <hr className="mt-4" />

        {carList}
        <Modal isOpen={this.state.togglePreview} toggle={this.togglePreview} className={this.props.className}>
          <ModalHeader toggle={this.togglePreview}>Image Preview</ModalHeader>
          <ModalBody>
            <img src={`data:image/png;base64,${this.state.selectedImage}`} alt={this.state.selectedImage} className="img-fluid w-100" />
          </ModalBody>
        </Modal>
      </div>
    );
  }
}

export default Products;
