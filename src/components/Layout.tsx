import React from 'react';
import Header from './Header.tsx';
import Sidebar from './Sidebar.tsx';

const Layout: React.FC = ({ children }) => {
  return (
    <>
      <Header />
      <Sidebar />
      {children}
    </>
  );
}

export default Layout;
