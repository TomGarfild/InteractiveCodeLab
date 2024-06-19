import React from 'react';
import { Link } from 'react-router-dom';
import logo from './logo.svg';
import "./Header.css";

const Header = ({ hideLoginButton }) => {
  return (
    <div className="header">
      <Link to="/"><img src={logo} alt="logo" className="logo" /></Link>
      {!hideLoginButton && (
        <Link to="/login"><button className="login-button">Login</button></Link>
      )}
    </div>
  );
};

export default Header;
