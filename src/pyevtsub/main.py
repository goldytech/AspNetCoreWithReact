import json
from time import sleep

from cloudevents.sdk.event import v1
from dapr.clients.grpc._response import TopicEventResponse
from dapr.ext.grpc import App

app = App()
should_retry = True  # To control whether dapr should retry sending a message


@app.subscribe(pubsub_name='rabbitmq-pubsub', topic='ordersubmitted')
def order_submit_handler(event: v1.Event) -> TopicEventResponse:
    global should_retry
    data = json.loads(event.Data())
    print(f'Order received: id={data["id"]} '
          f'content_type="{event.content_type}"', flush=True)
    if should_retry:
        should_retry = False  # we only retry once in this example
        sleep(0.5)  # add some delay to help with ordering of expected logs
        return TopicEventResponse('retry')
    return TopicEventResponse('success')

app.run(50051)

 # dapr run --app-id python-subscriber --app-protocol grpc --app-port 50051 --components-path ./dapr-components python3 main.py