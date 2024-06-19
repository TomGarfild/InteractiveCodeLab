import React from 'react';
import { Link } from 'react-router-dom';
import './LoginPage.css';
import Header from './Header';
import api from './api';

const LoginPage = () => {
  const handleSubmit = async (event) => {
    event.preventDefault();
    
    try {
      const response = await api.post('/user/login', {
        email: event.target.email.value,
        password: event.target.password.value,
      });

      localStorage.setItem('token', response.data);
      window.location.href = '/';
    } catch (error) {
      console.error('Login error:', error);
      alert('An error occurred during login');
    }
  };

  const handleGithubLogin = () => {
    // Redirect to GitHub OAuth 2.0 authorization URL
  };

  const handleGoogleLogin = () => {
    // Redirect to Google OAuth 2.0 authorization URL
  };

  return (
    <>
      <Header hideLoginButton />
      <div className="login-page">
        <h2>Login</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="email">Email:</label>
            <input type="email" id="email" name="email" required />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password:</label>
            <input type="password" id="password" name="password" required />
          </div>
          <div className="login-forgot-password">
            <button type="submit" className="login-button">Login</button>
          </div>
        </form>
        <div className="signup-link">
          Don't have an account? <Link to="/signup">Sign up</Link>
        </div>
      </div>
    </>
  );
};

export default LoginPage;

/*


            <Link to="/forgot-password" className="forgot-password-link">Forgot Password?</Link>

        <div className="separator">or</div>
        <div className="social-login">
          <button className="github-login" onClick={handleGithubLogin}>
            <img src="/github-logo.png" alt="GitHub" /> GitHub
          </button>
          <button className="google-login" onClick={handleGoogleLogin}>
            <img src="/google-logo.png" alt="Google" /> Google
          </button>
        </div>
*/
