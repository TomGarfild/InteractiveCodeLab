import React from 'react';
import { Link } from 'react-router-dom';
import './AlgorithmPreviewCard.css'

const AlgorithmPreviewCard = ({ algorithm }) => {
  return (
    <div className="algorithm-card">
      <h3 className="prev-title">{algorithm.title}</h3>
      <p className="prev-description">{algorithm.description}</p>
      <Link to={`/algorithm/${algorithm.id}`}>
        <button className="view-details">View Details</button>
      </Link>
    </div>
  );
};

export default AlgorithmPreviewCard;
