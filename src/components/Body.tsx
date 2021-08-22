import { Subscribe } from '@react-rxjs/core';
import MediaList from './MediaList';

const Body: React.FC = () => {
  return (
    <Subscribe fallback={<span>Loading Media...</span>}>
      <MediaList/>
    </Subscribe>
  );
};

export default Body;
