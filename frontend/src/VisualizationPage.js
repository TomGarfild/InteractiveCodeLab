import React, { useState } from 'react';
import api from './api';

const VisualizationPage = ({ id, selectedLanguage, code }) => {
  const [showVisualization, setShowVisualization] = useState(false);
  const [regime, setRegime] = useState('');
  const [customData, setCustomData] = useState('');

  const handleVisualize = () => {
    setShowVisualization(true);
  };

  const handleStartVisualization = async () => {
    try {
      const data = {
        selectedLanguage: selectedLanguage,
        code: code,
        regime: regime,
      };

      if (regime === 'custom') {
        data.customData = customData.split(',').map(Number);
      }

      await api.post(`/algorithm-visualization/visualize/${id}`, data);
    } catch (error) {
      console.error('Error visualizing algorithm:', error);
    }
  };

  return (
    <div>
      <button onClick={handleVisualize}>Visualize</button>

      {showVisualization && (
        <div>
          <label>
            Select Regime:
            <select value={regime} onChange={(e) => setRegime(e.target.value)}>
              <option value="">Select a regime</option>
              <option value="custom">Custom</option>
              <option value="test_samples">Test Samples</option>
              <option value="generated_large_data_set">Generated Large Data Set</option>
            </select>
          </label>

          {regime === 'custom' && (
            <div>
              <label>
                Custom Data (comma separated values):
                <input
                  type="text"
                  value={customData}
                  onChange={(e) => setCustomData(e.target.value)}
                />
              </label>
            </div>
          )}

          {regime && (
            <button onClick={handleStartVisualization}>Start Visualization</button>
          )}
        </div>
      )}
    </div>
  );
};

export default VisualizationPage;
