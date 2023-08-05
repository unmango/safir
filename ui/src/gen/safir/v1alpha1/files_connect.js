// @generated by protoc-gen-connect-es v0.12.0 with parameter "target=js+dts"
// @generated from file safir/v1alpha1/files.proto (package safir.v1alpha1, syntax proto3)
/* eslint-disable */
// @ts-nocheck

import { FilesServiceDiscoverRequest, FilesServiceDiscoverResponse, FilesServiceListRequest, FilesServiceListResponse } from "./files_pb.js";
import { MethodKind } from "@bufbuild/protobuf";

/**
 * @generated from service safir.v1alpha1.FilesService
 */
export const FilesService = {
  typeName: "safir.v1alpha1.FilesService",
  methods: {
    /**
     * @generated from rpc safir.v1alpha1.FilesService.List
     */
    list: {
      name: "List",
      I: FilesServiceListRequest,
      O: FilesServiceListResponse,
      kind: MethodKind.Unary,
    },
    /**
     * @generated from rpc safir.v1alpha1.FilesService.Discover
     */
    discover: {
      name: "Discover",
      I: FilesServiceDiscoverRequest,
      O: FilesServiceDiscoverResponse,
      kind: MethodKind.Unary,
    },
  }
};
