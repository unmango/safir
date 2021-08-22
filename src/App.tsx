import { Subscribe } from '@react-rxjs/core';
import React from 'react';
import './App.css';
import Body from './components/Body';
import Sidebar from './components/Sidebar';

const App: React.FC = () => {
  return (
    <>
      <Sidebar />
      <Subscribe fallback={<span>loading...</span>}>
        <Body />
      </Subscribe>
    </>
  );
};

export default App;
