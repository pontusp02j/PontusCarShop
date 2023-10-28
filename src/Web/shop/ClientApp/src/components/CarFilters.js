import React, { Component } from 'react';
import { CFormSelect } from '@coreui/react';

export class CarFilters extends Component {
  constructor(props) {
    super(props);

    this.state = {
      filterValue: this.props.currentFilter || '',
      statusFilterValue: this.props.currentStatusFilter || '',
      searchValue: null
    };
    
    this.handleChange = this.handleChange.bind(this);
    this.handleKeyDown = this.handleKeyDown.bind(this);
  }

  componentDidUpdate(prevProps) {
    if (this.props.currentFilter !== prevProps.currentFilter) 
    {
      this.setState({ filterValue: this.props.currentFilter || '' });  
    }

    if(this.props.currentStatusFilter !== prevProps.currentStatusFilter) 
    {
      this.setState({ statusFilterValue: this.props.currentStatusFilter || '' });
    }
  }

  handleChange(event) {
    const selectedValue = event.target.value;
    if (selectedValue !== this.state.filterValue && event.target.name === 'type') 
    {
      this.setState({ filterValue: selectedValue });
      this.props.onFilterChange(selectedValue);
    }

    if (selectedValue !== this.state.statusFilterValue && event.target.name === 'status') 
    {
      this.setState({ statusFilterValue: selectedValue });
      this.props.onStatusChange(selectedValue);
    }
  }

  handleKeyDown(event) {
    if (event.key === 'Enter') {
      const searchValue = this.state.searchValue;
      
      this.props.onSearchChange(searchValue);
      this.setState({ searchValue: null }, () => {
        event.target.value = null;
      });
    }
  }

  render() {
    const carTypeOptions = this.props.filterType.map((type, index) => (
      <option key={index} value={type}>
        {type}
      </option>
    ));

    const carTypeStatusOptions = this.props.filterStatusType.map((status, index) => (
      <option key={index} value={status}>
        {(status === 1 ? "Sold" : "Sale")}
      </option>
    ));

    return (
      <div>
        <CFormSelect
          name="type"
          onChange={this.handleChange}
          value={this.state.filterValue}
        >
          <option value="">Filter on car type</option>
          {carTypeOptions}
        </CFormSelect>

        <CFormSelect
        className='mt-2'
          name="status"
          onChange={this.handleChange}
          value={this.state.statusFilterValue}
        >
          <option value="">Filter on status</option>
          {carTypeStatusOptions}
        </CFormSelect>

        <div className="input-group mt-2">
          <input type="text" className="form-control" placeholder="Search" 
          onChange={(e) => this.setState({ searchValue: e.target.value })}
          onKeyDown={this.handleKeyDown}  />
        </div>
      </div>
    );
  }
}

export default CarFilters;
