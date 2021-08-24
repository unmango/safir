import React from 'react';
import './App.css';
import Body from './components/Body';
import Sidebar from './components/Sidebar';

const App: React.FC = () => {
  return (
    <div className="App">
      <div className="Header">
        <span>Header</span>
      </div>
      <div className="Sidebar">
        <Sidebar aria-label="sidebar" />
      </div>
      <div className="Body">
        <Body aria-label="body" />
      </div>
    </div>
  );
};

export default App;
