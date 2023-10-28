import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { HomeOutlined } from '@ant-design/icons';

export const UserRole = Object.freeze({
  USER: 0,
  ADMIN: 1,
});

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      userId: -1,
      user: {},
    };
  }

  componentDidMount() {
    const user = localStorage.getItem('user');
    if (user) {
      this.setState({ user: JSON.parse(user) });
    }
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/" style={{ display: 'flex', alignItems: 'center' }}>
            <HomeOutlined title="Home" style={{ marginRight: '10px' }} />
            {this.state.user?.username && (
              <div style={{ display: 'flex', alignItems: 'center', marginLeft: '10px' }}>
                <div
                  style={{
                    height: '7px',
                    width: '7px',
                    backgroundColor: 'green',
                    borderRadius: '50%',
                    position: 'relative',
                  }}
                >
                  <div
                    style={{
                      position: 'absolute',
                      width: '100%',
                      height: '100%',
                      borderRadius: '50%',
                      border: '2px solid green',
                      boxShadow: '0 0 5px 2px green',
                      content: '',
                    }}
                  />
                </div>
                <span style={{ marginLeft: '10px', fontSize: '16px' }}>{this.state.user.username} {this.state.user?.permissionLevel === UserRole.ADMIN && '(Admin)'}</span>
              </div>
            )}
          </NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Products</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/registration">Registration</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} className="text-dark" to={`/users/admin/${this.state.userId}/change-password`}>Change password</NavLink>
              </NavItem>
              {this.state.user?.permissionLevel === UserRole.ADMIN &&
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/subscriptions">Subscriptions</NavLink>
                </NavItem>
              }
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/users">Users</NavLink>
              </NavItem>
              {this.state.user?.id &&
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to={`/users/${this.state.user.id}`}>My Page</NavLink>
                </NavItem>
              }
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/report">Report</NavLink>
              </NavItem>
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }
}
