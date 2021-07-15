import React from 'react';
import './App.css';
import Body from './components/Body';
import Sidebar from './components/Sidebar';

const App: React.FC = () => {
  return (
    <>
      <Sidebar/>
      <Body/>
    </>
  );
}

export default App;
