import { bind } from '@react-rxjs/core';
import { toArray } from 'rxjs';
import { media } from '../services';

const [useFiles] = bind(media.list().pipe(toArray()));

const MediaList: React.FC = () => {
  const files = useFiles();

  return (
    <div>
      {files.map((file) => (
        <div key={file.getPath()}>
          <span>File: {file.getPath()}</span>
        </div>
      ))}
    </div>
  );
}

export default MediaList;