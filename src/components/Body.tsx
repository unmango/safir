import { bind } from '@react-rxjs/core';
import { toArray } from 'rxjs';
import { media } from '../services';

const [useFiles] = bind(media.list().pipe(toArray()));

const Body: React.FC = () => {
  const files = useFiles();

  return (
    <div>
      <h4>Body</h4>
      {files.map((file) => (
        <div key={file.getPath()}>
          <span>File: {file.getPath()}</span>
        </div>
      ))}
    </div>
  );
};

export default Body;
