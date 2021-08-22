import '@testing-library/jest-dom/extend-expect';
import { render, screen } from '@testing-library/react';
import App from './App';

jest.mock('./components/Body.tsx');
jest.mock('./components/Sidebar.tsx');

test('renders learn react link', () => {
  render(<App />);

  const linkElement = screen.getByLabelText(/body/i);

  expect(linkElement).toBeInTheDocument();
});
