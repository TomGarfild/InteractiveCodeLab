import React from 'react';
import { Link } from 'react-router-dom';
import './SignupPage.css';
import Header from './Header';
import api from './api';

const SignupPage = () => {
  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const response = await api.post('/user/register', {
        firstName: event.target.firstName.value,
        lastName: event.target.lastName.value,
        email: event.target.email.value,
        password: event.target.password.value,
      });

      localStorage.setItem('token', response.data);
      window.location.href = '/';
    } catch (error) {
      console.error('Registration error:', error);
      alert('An error occurred during registration');
    }
  };

  const handleGithubSignup = () => {
    // Redirect to GitHub OAuth 2.0 authorization URL
  };

  const handleGoogleSignup = () => {
    // Redirect to Google OAuth 2.0 authorization URL
  };

  return (
    <>
      <Header hideLoginButton />
      <div className="signup-page">
        <h2>Sign Up</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="firstName">First Name:</label>
            <input type="text" id="firstName" name="firstName" required />
          </div>
          <div className="form-group">
            <label htmlFor="lastName">Last Name:</label>
            <input type="text" id="lastName" name="lastName" required />
          </div>
          <div className="form-group">
            <label htmlFor="email">Email:</label>
            <input type="email" id="email" name="email" required />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password:</label>
            <input type="password" id="password" name="password" required />
          </div>
          <button type="submit" className="signup-button">Sign Up</button>
        </form>
        <div className="login-link">
          Already have an account? <Link to="/login">Login</Link>
        </div>
      </div>
    </>
  );
};

export default SignupPage;

/*

        <div className="separator">or</div>
        <div className="social-signup">
          <button className="github-signup" onClick={handleGithubSignup}>
            <img src="/github-logo.png" alt="GitHub" /> GitHub
          </button>
          <button className="google-signup" onClick={handleGoogleSignup}>
            <img src="/google-logo.png" alt="Google" /> Google
          </button>
        </div>
*/
