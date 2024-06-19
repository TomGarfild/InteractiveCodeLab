import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import './AlgorithmPage.css';
import api from './api';

const AlgorithmPage = () => {
  const { id } = useParams();
  const [algorithmDetails, setAlgorithmDetails] = useState({ title: '', description: '' });
  const [code, setCode] = useState('');
  const [selectedLanguage, setSelectedLanguage] = useState('python3');
  const [showVisualization, setShowVisualization] = useState(false);
  const [regime, setRegime] = useState('');
  const [customData, setCustomData] = useState('');
  const [visualizationSteps, setVisualizationSteps] = useState([]);
  const [currentStepIndex, setCurrentStepIndex] = useState(0);
  const [startedAutoPlay, setStartedAutoPlay] = useState(false);

  useEffect(() => {
    const fetchAlgorithmDetails = async () => {
      try {
        const response = await api.get(`/algorithms/${id}/${selectedLanguage}`);
        const data = response.data;
        setAlgorithmDetails(data);
        setCode(data.code);
      } catch (error) {
        console.error('Error fetching algorithm details:', error);
      }
    };
    fetchAlgorithmDetails();
  }, [id, selectedLanguage]);

  const handleSave = async () => {
    try {
      await api.post(`/user-code/save/${id}`, {
        code: code,
        selectedLanguage: selectedLanguage,
      });
    } catch (error) {
      console.error('Error saving code:', error);
    }
  };

  const handleCompile = async () => {
    try {
      await api.post(`/user-code/compile/${id}/${selectedLanguage}`);
    } catch (error) {
      console.error('Error compiling code:', error);
    }
  };

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

      // await api.post(`/algorithm-visualization/visualize/${id}`, data);

      
      setCurrentStepIndex(0);
    } catch (error) {
      console.error('Error visualizing algorithm:', error);
    }
  };

  const handleNextStep = () => {
    if (currentStepIndex < visualizationSteps.length - 1) {
      setCurrentStepIndex(currentStepIndex + 1);
    }
  };

  const handlePreviousStep = () => {
    if (currentStepIndex > 0) {
      setCurrentStepIndex(currentStepIndex - 1);
    }
  };

  const handleAutoPlay = () => {
    setStartedAutoPlay(true)
    const id = setInterval(() => {
      setCurrentStepIndex((prevIndex) => {
        if (prevIndex < visualizationSteps.length - 1) {
          return prevIndex + 1;
        } else {
          clearInterval(id);
          return prevIndex;
        }
      });
    }, 500);
    setStartedAutoPlay(false)
  };

  return (
    <div className="algorithm-page">
      <h1 className="algo-title">{algorithmDetails.title}</h1>
      <p className="algo-description">{algorithmDetails.description}</p>
      <textarea
        value={code}
        onChange={(e) => setCode(e.target.value)}
        placeholder="Enter your algorithm code here..."
        rows={10}
        cols={50}
        className="textarea"
      ></textarea>
      <select
        value={selectedLanguage}
        onChange={(e) => setSelectedLanguage(e.target.value)}
        className="dropdown"
      >
        <option value="python3">Python3</option>
        <option value="c#">C#</option>
        <option value="java">Java</option>
      </select>
      <div className="button-container">
        <button onClick={handleSave} className="button save">
          <i className="save-icon"></i>
          <span className="button-text">Save</span>
        </button>
        <button onClick={handleCompile} className="button compile">
          <i className="compile-icon"></i>
          <span className="compile-text">Compile</span> 
        </button>
        <button onClick={handleVisualize} className="button visualize">
          <i className="visualize-icon"></i>
          <span className="button-text">Visualize</span>
        </button>
      </div>

      {showVisualization && (
        <div className="visualization-window">
          <label>
            Select Regime:
            <select value={regime} onChange={(e) => setRegime(e.target.value)}>
              <option value="">Select a regime</option>
              <option value="custom">Custom Input</option>
              <option value="test1">Test Sample 1</option>
              <option value="test2">Test Sample 2</option>
              <option value="test3">Test Sample 3</option>
              <option value="generated_large_data_set">Generated Data Set</option>
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
            <div className="visualization-controls">
              <button onClick={handleStartVisualization} className="control-button">Set Regime</button>
              <button className="control-button" onClick={handleAutoPlay}>{startedAutoPlay ? 'Stop Auto Play' : 'Auto Play'}</button>
              <button className="control-button" onClick={handleNextStep}>Step Forward</button>
              <button className="control-button" onClick={handlePreviousStep}>Step Back</button>
            </div>
          )}

          <div className="visualization-output">
            {visualizationSteps.length > 0 && (
              <div>
                <div className="array-state">
                  {visualizationSteps[currentStepIndex].DataState.map((num, index) => (
                    <div key={index} className="array-element">
                      {num}
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default AlgorithmPage;
