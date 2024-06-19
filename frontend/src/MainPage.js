import React, { useEffect, useState } from 'react';
import { Route, Routes } from 'react-router-dom';
import Header from './Header';
import AlgorithmPreviewCard from './AlgorithmPreviewCard';
import AlgorithmPage from './AlgorithmPage';
import './MainPage.css';
import api from './api'
import VisualizationPage from './VisualizationPage';

const MainPage = () => {
  const [algorithms, setAlgorithms] = useState([]);

  useEffect(() => {
    fetchAlgorithms();
  }, []);

  const fetchAlgorithms = async () => {
    try {
      const response = await api.get('/algorithms/all-previews');

      setAlgorithms(response.data);
    } catch (error) {
      console.error('Error fetching algorithms:', error);
    }
  };

  return (
    <div className="main-page">
      <Header />
      <Routes>
        <Route path="/" element={<div className="algorithm-cards">
          {algorithms.map((algorithm) => (
            <AlgorithmPreviewCard key={algorithm.id} algorithm={algorithm} />
          ))}
        </div>} />
        <Route path="/algorithm/:id" element={<AlgorithmPage />} />
        <Route path="/algorithm/:id/visualization" element={<VisualizationPage />} />
      </Routes>
    </div>
  );
};

export default MainPage;
