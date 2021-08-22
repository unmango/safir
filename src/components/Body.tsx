import { Subscribe } from '@react-rxjs/core';
import { ErrorBoundary, FallbackProps } from 'react-error-boundary';
import MediaList from './MediaList';

const ErrorFallback: React.FC<FallbackProps> = ({
  error,
  resetErrorBoundary,
}) => {
  return (
    <div role="alert">
      <p>Something went wrong:</p>
      <pre>{error?.message}</pre>
      <button onClick={resetErrorBoundary}>Try again</button>
    </div>
  );
};

const Body: React.FC = () => {
  return (
    <ErrorBoundary FallbackComponent={ErrorFallback}>
      <Subscribe fallback={<span>Loading Media...</span>}>
        <MediaList />
      </Subscribe>
    </ErrorBoundary>
  );
};

export default Body;
