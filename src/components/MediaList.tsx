import { bind } from '@react-rxjs/core';
import { toArray } from 'rxjs';
import { media } from '../services';
import Media from './manager/Media';

const [useFiles] = bind(media.list().pipe(toArray()));

const MediaList: React.FC = () => {
  const files = useFiles();

  return (
    <div>
      {files.map((file) => (
        <div key={file.getPath()}>
          <Media item={file} />
        </div>
      ))}
    </div>
  );
};

export default MediaList;
