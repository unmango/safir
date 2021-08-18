import { MediaClient } from '@unmango/safir-grpc-client/dist/manager';

const baseUrl = process.env.REACT_APP_MANAGER_URL ?? '';
export const media = new MediaClient(baseUrl);
