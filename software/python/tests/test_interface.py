from harp.device import core, timestampgeneratorgen3


def test_device_identity_matches_the_metadata():
    assert timestampgeneratorgen3.DEVICE_NAME == "TimestampGeneratorGen3"
    assert timestampgeneratorgen3.WHO_AM_I == 1158


def test_register_map_covers_the_core_and_application_registers():
    # Application registers start at address 32, so the map is only complete if
    # the core registers were merged into it as well.
    assert timestampgeneratorgen3.REGISTER_MAP
    assert all(address in timestampgeneratorgen3.REGISTER_MAP for address in core.REGISTER_MAP)
    assert any(address >= 32 for address in timestampgeneratorgen3.REGISTER_MAP)
